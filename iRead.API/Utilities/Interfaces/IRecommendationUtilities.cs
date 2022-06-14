using iRead.API.Models.Recommendation;
using iRead.RecommendationSystem.Models;

namespace iRead.API.Utilities.Interfaces
{
    public interface IRecommendationUtilities
    {
        public Task TrainModel();
        public Task<string> MakePrediction(RecommendationInput input);
        public Task<IEnumerable<RecommendedBook>> GetRecommendedBooks(int userId);

        public Task<IEnumerable<int>> GetRecommendedBooksBasedOnFavorites(int userId, IEnumerable<int> recommendedByEngine, int maxBooksNeeded = 6, IEnumerable<int> excludedIds = null);
    }
}
