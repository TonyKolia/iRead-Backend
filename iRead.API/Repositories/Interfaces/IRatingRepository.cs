using iRead.API.Models;
using iRead.API.Models.Rating;

namespace iRead.API.Repositories.Interfaces
{
    public interface IRatingRepository
    {
        Task<BookRatingsResponse> GetBookRatings(int bookId);
        Task<RatingResponse> UpdateRating(NewRating rating);
        Task<Rating> GetRating(int userId, int bookId);
        Task DeleteRating(int userId, int bookId);
        Task<Rating> CreateRating(NewRating rating);
    }
}
