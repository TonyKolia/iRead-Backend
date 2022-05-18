using iRead.API.Models;
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

        public async Task<BookRatingsResponse> GetBookRatings(int bookId)
        {
            return await _db.Books.Where(x => x.Id == bookId).Select(x => new BookRatingsResponse
            {
                BookId = x.Id,
                BookTitle = x.Title,
                Ratings = x.Ratings.Select(r => new RatingResponse 
                {
                    Username = r.User.Username,
                    DateAdded = r.DateAdded,
                    Comment = r.Comment ?? "",
                    Rating = r.Rating1
                }).ToList()
            }).FirstOrDefaultAsync();
        }

        public async Task<Rating> GetRating(int userId, int bookId)
        {
            return await _db.Ratings.FirstOrDefaultAsync(x => x.UserId == userId && x.BookId == bookId);
        }

        public async Task<RatingResponse> UpdateRating(NewRating rating)
        {
            var ratingToUpdate = await GetRating(rating.UserId, rating.BookId);
            if (rating.Rating != ratingToUpdate.Rating1)
                ratingToUpdate.Rating1 = rating.Rating;

            if (rating.Comment != ratingToUpdate.Comment)
                ratingToUpdate.Comment = rating.Comment;

            _db.Entry(ratingToUpdate).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return new RatingResponse();
        }

        public async Task DeleteRating(int userId, int bookId)
        {
            var ratingToDelete = await GetRating(userId, bookId);
            _db.Entry(ratingToDelete).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
        }
    }
}
