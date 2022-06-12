using iRead.API.Models.Recommendation;
using iRead.API.Repositories.Interfaces;
using iRead.API.Utilities.Interfaces;
using iRead.RecommendationSystem;
using iRead.RecommendationSystem.Models;
using Microsoft.Extensions.ML;

namespace iRead.API.Utilities
{
    public class RecommendationUtilities : IRecommendationUtilities
    {
        private readonly Recommender _recommender;
        private readonly iReadDBContext _db;
        private readonly PredictionEnginePool<RecommendationInput, RecommendationOutput> _predictionEngine;
        private readonly IUserRepository _userRepository;

        public RecommendationUtilities(IUserRepository _userRepository, PredictionEnginePool<RecommendationInput, RecommendationOutput> _predictionEngine, Recommender _recommender, iReadDBContext _db)
        {
            this._recommender = _recommender;
            this._db = _db;
            this._predictionEngine = _predictionEngine;
            this._userRepository = _userRepository;
        }

        public async Task<string> MakePrediction(RecommendationInput input)
        {
            var prediction = _predictionEngine.Predict(input);
            return string.Format("UserId: {0} - BookId: {1} - Prediction: {2}", input.UserId, input.BookId, prediction.Rating);
        }

        public async Task<IEnumerable<RecommendedBook>> GetRecommendedBooks(int userId)
        {
            var predictions = _db.Books.Where(book => !book.Orders.Any(order => order.UserId == userId && order.Books.Contains(book))).AsEnumerable().Select(book => 
            {
                var prediction = _predictionEngine.Predict(new RecommendationInput(Convert.ToUInt16(userId), Convert.ToUInt16(book.Id)));
                if (!float.IsNaN(prediction.Rating))
                    return new RecommendedBook(book.Id, (float)Math.Round(prediction.Rating, 2));

                return new RecommendedBook(book.Id, 0);
            });

            return predictions.Where(x => x.PredictedRating > 0).OrderByDescending(x => x.PredictedRating);
        }

        public async Task<IEnumerable<int>> GetRecommendedBooksBasedOnFavorites(int userId, int maxBooksNeeded = 6)
        {
            var userFavoriteCategories = await _userRepository.GetFavoriteCategories(userId);
            //var userFavoriteAuthors = user.Authors.Select(x => x.Id).ToList();

            var booksPerCategory = 6 / userFavoriteCategories.Count();

            var recommendedBooks = new List<int>();
            foreach(var category in userFavoriteCategories)
            {
                if (recommendedBooks.Count >= maxBooksNeeded)
                    break;

                var books = _db.Books.Where(x => x.Categories.Any(c => c.Id == category) && !x.Orders.Any(order => order.UserId == userId && order.Books.Contains(x))).Take(booksPerCategory).AsEnumerable();
                recommendedBooks.AddRange(books.Select(x => x.Id));
            }

            if (maxBooksNeeded > recommendedBooks.Count)
                recommendedBooks = recommendedBooks.Take(maxBooksNeeded).ToList();

            return recommendedBooks;
        }

        public async Task TrainModel()
        {
            var trainingData = _db.GetRecommenderTrainingData().AsEnumerable().TransformToTrainingData();
            _recommender.SetTrainingData(trainingData);
            _recommender.TrainModel();
        }


    }
}
