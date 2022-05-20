using iRead.API.Models;
using iRead.API.Repositories.Interfaces;
using LinqKit;
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
                Stock = x.BooksStock.Stock,
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
                Stock = x.BooksStock.Stock,
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
            return await _db.Books.Where(x => x.Categories.Any(c => c.Id == categoryId)).Select(x => new BookResponse
            {
                Id = x.Id,
                Title = x.Title,
                ISBN = x.Isbn,
                PageCount = x.PageCount,
                Description = x.Description,
                ImagePath = x.ImagePath ?? "",
                PublishDate = x.PublishDate.Value,
                Stock = x.BooksStock.Stock,
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

        public async Task<IEnumerable<BookResponse>> GetBooksByPublishers(IEnumerable<int> publishers)
        {
            return await _db.Books.Where(x => x.Publishers.Any(a => publishers.Contains(a.Id))).Select(x => new BookResponse
            {
                Id = x.Id,
                Title = x.Title,
                ISBN = x.Isbn,
                PageCount = x.PageCount,
                Description = x.Description,
                ImagePath = x.ImagePath ?? "",
                PublishDate = x.PublishDate.Value,
                Stock = x.BooksStock.Stock,
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

        public async Task<IEnumerable<BookResponse>> GetBooksByAuthors(IEnumerable<int> authors)
        {
            return await _db.Books.Where(x => x.Authors.Any(a => authors.Contains(a.Id))).Select(x => new BookResponse
            {
                Id = x.Id,
                Title = x.Title,
                ISBN = x.Isbn,
                PageCount = x.PageCount,
                Description = x.Description,
                ImagePath = x.ImagePath ?? "",
                PublishDate = x.PublishDate.Value,
                Stock = x.BooksStock.Stock,
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

        public async Task<IEnumerable<BookResponse>> GetBooksByFilters(IEnumerable<int> authors, IEnumerable<int> publishers, int? minYear, int? maxYear, int? categoryId)
        {
            var books = _db.Books.AsQueryable();
            var predicate = PredicateBuilder.New<Book>();
            var hasFilters = false;

            if (minYear.HasValue)
            {
                books = books.Where(x => x.PublishDate.Value.Year >= minYear.Value);
                //predicate = predicate.And(x => x.PublishDate.Value.Year >= minYear.Value);
                hasFilters = true;
            }

            if (maxYear.HasValue)
            {
                hasFilters = true;
                //predicate = predicate.And(x => x.PublishDate.Value.Year <= maxYear.Value);
                books = books.Where(x => x.PublishDate.Value.Year <= maxYear.Value);
            }

            if (authors.Count() > 0)
            {
                hasFilters = true;
                //predicate = predicate.And(x => x.Authors.Any(a => authors.Contains(a.Id)));
                books = books.Where(x => x.Authors.Any(a => authors.Contains(a.Id)));
            }

            if (publishers.Count() > 0)
            {
                //predicate = predicate.And(x => x.Publishers.Any(p => publishers.Contains(p.Id)));
                books = books.Where(x => x.Publishers.Any(p => publishers.Contains(p.Id)));
                hasFilters = true;
            }
                
            if (categoryId.HasValue)
            {
                //predicate = predicate.And(x => x.Categories.Any(c => c.Id == categoryId.Value));
                books = books.Where(x => x.Categories.Any(c => c.Id == categoryId.Value));
                hasFilters = true;
            }
                
            //if(hasFilters)
            //    books = books.AsExpandableEFCore().Where(predicate);

            return await books.Select(x => new BookResponse
            {
                Id = x.Id,
                Title = x.Title,
                ISBN = x.Isbn,
                PageCount = x.PageCount,
                Description = x.Description,
                ImagePath = x.ImagePath ?? "",
                PublishDate = x.PublishDate.Value,
                Stock = x.BooksStock.Stock,
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

        public async Task<IEnumerable<BookResponse>> GetBooksByIds(IEnumerable<int> ids)
        {
            return await _db.Books.Where(x => ids.Contains(x.Id)).Select(x => new BookResponse
            {
                Id = x.Id,
                Title = x.Title,
                ISBN = x.Isbn,
                PageCount = x.PageCount,
                Description = x.Description,
                ImagePath = x.ImagePath ?? "",
                PublishDate = x.PublishDate.Value,
                Stock = x.BooksStock.Stock,
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

        public async Task<int> GetBookStock(int bookId)
        {
            return await _db.BooksStocks.Where(x => x.BookId == bookId).Select(x => x.Stock).FirstOrDefaultAsync();
        }

        public async Task UpdateBookStock(IEnumerable<int> books)
        {
            foreach(var book in books)
            {
                var currentStock = await _db.BooksStocks.FirstOrDefaultAsync(x => x.BookId == book);
                currentStock.Stock--;
                _db.Entry(currentStock).State = EntityState.Modified;
            }

            await _db.SaveChangesAsync();
        }

        public async Task<int> GetMinPublishYear()
        {
            return (await _db.Books.MinAsync(x => x.PublishDate)).Value.Year;
        }

        public async Task<int> GetMaxPublishYear()
        {
            return (await _db.Books.MaxAsync(x => x.PublishDate)).Value.Year;
        }
    }
}
