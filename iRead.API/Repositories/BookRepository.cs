using iRead.API.Models;
using iRead.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace iRead.API.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly iReadDBContext _db;

        public BookRepository(iReadDBContext db)
        {
            this._db = db;
        }

        public async Task<IEnumerable<BookResponse>> GetAllBooks()
        {
            return await _db.Books.Select(x => new BookResponse
            {
                Id = x.Id,
                Title = x.Title,
                ISBN = x.Isbn,
                PageCount = x.PageCount,
                Description = x.Description,
                ImagePath = x.ImagePath ?? "",
                PublishDate = x.PublishDate.Value,
                Authors = x.Authors.Select(a => new AuthorResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    Surname = a.Surname,
                    Birthdate = a.Birthdate
                }),
                Categories = x.Categories.Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Description = c.Description ?? ""
                }),
                Ratings = x.Ratings.Select(r => new RatingResponse
                {
                    Username = r.User.Username,
                    Rating = r.Rating1,
                    Comment = r.Comment ?? "",
                    DateAdded = r.DateAdded
                }),
                Publishers = x.Publishers.Select(p => new PublisherResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description ?? ""
                })
            }).ToListAsync();
        }

        public async Task<BookResponse> GetBook(int id)
        {
            return await _db.Books.Select(x => new BookResponse
            {
                Id = x.Id,
                Title = x.Title,
                ISBN = x.Isbn,
                PageCount = x.PageCount,
                Description = x.Description,
                ImagePath = x.ImagePath ?? "",
                PublishDate = x.PublishDate.Value,
                TotalRatings = x.Ratings.Count(),
                Rating = x.Ratings.Count() > 0 ? Math.Round(x.Ratings.Average(r => r.Rating1),2) : 0,
                Authors = x.Authors.Select(a => new AuthorResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    Surname = a.Surname,
                    Birthdate = a.Birthdate
                }),
                Categories = x.Categories.Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Description = c.Description ?? ""
                }),
                Ratings = x.Ratings.Select(r => new RatingResponse
                {
                    Username = r.User.Username,
                    Rating = r.Rating1,
                    Comment = r.Comment ?? "",
                    DateAdded = r.DateAdded
                }),
                Publishers = x.Publishers.Select(p => new PublisherResponse 
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description ?? ""
                })
            }).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<BookResponse>> GetBooksByCategory(int categoryId)
        {
            return new List<BookResponse>();
        }
    }
}
