using iRead.API.Models.Recommendation;
using iRead.API.Repositories.Interfaces;
using iRead.API.Utilities.Interfaces;
using iRead.RecommendationSystem;
using iRead.RecommendationSystem.Models;
using Microsoft.Extensions.ML;
using System.Linq;
using System;

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

        public async Task<IEnumerable<int>> GetRecommendedBooksBasedOnFavorites(int userId, IEnumerable<int> recommendedByEngine, int maxBooksNeeded = 6, IEnumerable<int> excludedIds = null)
        {
            var userFavoriteCategories = await _userRepository.GetFavoriteCategories(userId);
            var userFavoriteAuthors = await _userRepository.GetFavoriteAuthors(userId);
            var recommendedBooks = new List<int>();

            //exclude books already read
            var books = _db.Books.Where(x => !x.Orders.Any(order => order.UserId == userId && order.Books.Contains(x))).AsQueryable();

            //exclude books already recommended by the engine
            if (recommendedByEngine.Count() > 0)
                books = books.Where(x => !recommendedByEngine.Contains(x.Id));

            //get books recommended by both (combined) favorite criteria
            var recommendedByCategoryAndAuthor = books.Where(x => x.Categories.Any(c => userFavoriteCategories.Contains(c.Id)) && x.Authors.Any(a => userFavoriteAuthors.Contains(a.Id))).Select(x => x.Id).AsEnumerable();
            if (excludedIds != null && excludedIds.Count() > 0)
                recommendedByCategoryAndAuthor = recommendedByCategoryAndAuthor.Where(x => !excludedIds.Contains(x));

            //if we find more than enough, return them
            if (recommendedByCategoryAndAuthor.Count() >= maxBooksNeeded)
                return recommendedByCategoryAndAuthor.Take(maxBooksNeeded);

            //if any found, exclude them from the next search to avoid duplicates
            if (recommendedByCategoryAndAuthor.Count() > 0)
            {
                recommendedBooks.AddRange(recommendedByCategoryAndAuthor);
                books = books.Where(x => !recommendedByCategoryAndAuthor.Contains(x.Id));
            }
                
            var booksPerCategory = 6 / userFavoriteCategories.Count();
            var categoryBooks = new List<int>();
            //randomly order books using a guid
            foreach (var category in userFavoriteCategories)
            {
                if(excludedIds != null && excludedIds.Count() > 0)
                    categoryBooks.AddRange(books.Where(x => x.Categories.Any(x => x.Id == category) && !excludedIds.Contains(x.Id)).Select(x => new { Guid = Guid.NewGuid().ToString(), x.Id }).OrderBy(x => x.Guid).Select(x => x.Id).Take(booksPerCategory).AsEnumerable());
                else
                    categoryBooks.AddRange(books.Where(x => x.Categories.Any(x => x.Id == category)).Select(x => new { Guid = Guid.NewGuid().ToString(), x.Id }).OrderBy(x => x.Guid).Select(x => x.Id).Take(booksPerCategory).AsEnumerable());
            }

            //randomly add the remaining recommended by category books
            recommendedBooks.AddRange(categoryBooks.RandomlyOrderList().Take(maxBooksNeeded - recommendedBooks.Count));

            if (recommendedBooks.Count > maxBooksNeeded)
                recommendedBooks = recommendedBooks.Take(maxBooksNeeded).ToList();

            return recommendedBooks;
        }

        public async Task TrainModel()
        {
            var trainingData = _db.GetRecommenderTrainingData().AsEnumerable().TransformToTrainingData();
            _recommender.SetTrainingData(trainingData);
            _recommender.TrainModel();


            var evaluationData = _db.GetRecommenderTrainingData().ToList();
            var realUsers = _db.Users.Where(x => !x.Username.StartsWith("user")).Select(x => x.Id).ToList();
            evaluationData = evaluationData.Where(x => realUsers.Contains(x.UserId)).ToList();
            _recommender.EvaluateModel(evaluationData.TransformToTrainingData());
        }
    }
}
