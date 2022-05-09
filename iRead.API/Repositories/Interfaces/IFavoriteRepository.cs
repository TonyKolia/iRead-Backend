using iRead.API.Models;
using iRead.API.Models.Favorite;

namespace iRead.API.Repositories.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<IEnumerable<FavoriteResponse>> GetUserFavorites(int userId);
        Task<FavoriteResponse> GetFavorite(int userId, int bookId);
        Task<FavoriteResponse> CreateFavorite(NewFavorite favorite);
    }
}
