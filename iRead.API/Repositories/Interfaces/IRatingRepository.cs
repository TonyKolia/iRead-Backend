using iRead.API.Models.Rating;

namespace iRead.API.Repositories.Interfaces
{
    public interface IRatingRepository
    {
        Task<IEnumerable<Rating>> GetBookRatings(int bookId);
        Task<Rating> CreateRating(NewRating rating);
    }
}
