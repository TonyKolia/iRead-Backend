using iRead.API.Models.Rating;
using iRead.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace iRead.API.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly iReadDBContext _db;

        public RatingRepository(iReadDBContext db)
        {
            this._db = db;
        }


        public async Task<Rating> CreateRating(NewRating rating)
        {
            var newRating = new Rating 
            { 
                BookId = rating.BookId,
                UserId = rating.UserId,
                Rating1 = rating.Rating,
                Comment = rating.Comment,
                DateAdded = DateTime.Now
            };

            _db.Entry(newRating).State = EntityState.Added;
            await _db.SaveChangesAsync();

            return newRating;
        }

        public async Task<IEnumerable<Rating>> GetBookRatings(int bookId)
        {
            return await _db.Ratings.Where(x => x.BookId == bookId).ToListAsync();
        }
    }
}
