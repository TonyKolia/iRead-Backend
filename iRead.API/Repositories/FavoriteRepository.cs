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
                BookRead = _db.Orders.Any(x => x.UserId == favorite.UserId && x.Books.Any(b => b.Id == favorite.BookId)) ? 1 : 0
            };

            _db.Entry(newFavorite).State = EntityState.Added;
            await _db.SaveChangesAsync();

            return await GetFavorite(newFavorite.UserId, newFavorite.BookId);
        }

        public async Task DeleteFavorite(int bookId, int userId)
        {
            var favoriteToDelete = await _db.Favorites.FirstOrDefaultAsync(x => x.UserId == userId && x.BookId == bookId);
            _db.Entry(favoriteToDelete).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
        }

        public async Task<bool> FavoriteExists(int bookId, int userId)
        {
            return await _db.Favorites.AnyAsync(x => x.BookId == bookId && x.UserId == userId);
        }

        public async Task<FavoriteResponse> GetFavorite(int userId, int bookId)
        {
            return await _db.Favorites.Select(x => new FavoriteResponse
            {
                DateAdded = x.DateAdded,
                BookRead = x.BookRead == 1,
                Book = new BookResponse { Id = x.Book.Id, Title = x.Book.Title, ImagePath = x.Book.ImagePath, Stock = x.Book.BooksStock.Stock },
                UserId = userId
            }).FirstOrDefaultAsync(x => x.UserId == userId && x.Book.Id == bookId);
        }

        public async Task<IEnumerable<FavoriteResponse>> GetUserFavorites(int userId)
        {
            return await _db.Favorites.Where(x => x.UserId == userId).Select(x => new FavoriteResponse
            {
                DateAdded = x.DateAdded,
                BookRead = x.BookRead == 1,
                Book = new BookResponse { Id = x.Book.Id, Title = x.Book.Title, ImagePath = x.Book.ImagePath, Stock = x.Book.BooksStock.Stock },
                UserId = userId
            }).ToListAsync();
        }

        public async Task<FavoriteResponse> UpdateFavorite(Favorite favorite)
        {
            _db.Entry(favorite).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return await GetFavorite(favorite.UserId, favorite.BookId);
        }
    }
}
