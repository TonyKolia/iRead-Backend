using iRead.API.Models;
using iRead.API.Models.Favorite;
using iRead.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace iRead.API.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly iReadDBContext _db;
        private readonly IBookRepository _bookRepository;

        public FavoriteRepository(iReadDBContext db, IBookRepository _bookRepository)
        {
            this._db = db;
            this._bookRepository = _bookRepository;
        }

        public async Task<FavoriteResponse> CreateFavorite(NewFavorite favorite)
        {
            var newFavorite = new Favorite
            {
                UserId = favorite.UserId,
                BookId = favorite.BookId,
                DateAdded = DateTime.Now,
                BookRead = 1
            };

            _db.Entry(newFavorite).State = EntityState.Added;
            await _db.SaveChangesAsync();

            return await GetFavorite(newFavorite.UserId, newFavorite.BookId);
        }

        public async Task<FavoriteResponse> GetFavorite(int userId, int bookId)
        {
            return await _db.Favorites.Select(x => new FavoriteResponse
            {
                DateAdded = x.DateAdded,
                BookRead = x.BookRead == 1,
                Book = _bookRepository.GetBook(bookId).Result,
                UserId = userId
            }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<FavoriteResponse>> GetUserFavorites(int userId)
        {
            return await _db.Favorites.Where(x => x.UserId == userId).Select(x => new FavoriteResponse 
            {
                DateAdded = x.DateAdded,
                BookRead = x.BookRead == 1,
                Book = _bookRepository.GetBook(x.BookId).Result,
                UserId = userId
            }).ToListAsync();
        }
    }
}
