﻿using iRead.API.Models;
using iRead.API.Repositories.Interfaces;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using iRead.API.Utilities;
using iRead.API.Utilities.Interfaces;

namespace iRead.API.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly iReadDBContext _db;
        private readonly IRecommendationUtilities _recommendationUtilities;
        private readonly IUserRepository _userRepository;

        public BookRepository(IUserRepository _userRepository, IRecommendationUtilities _recommendationUtilities, iReadDBContext db)
        {
            this._db = db;
            this._recommendationUtilities = _recommendationUtilities;
            this._userRepository = _userRepository;
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


        private async Task<IQueryable<Book>> SetupHomeBooksQuery(int? userId, string type)
        {
            var books = _db.Books.AsQueryable();
            var pageSize = 6;

            switch (type)
            {
                case "recommended":
                    if (userId.HasValue)
                    {
                        var recommendedBooks = (await _recommendationUtilities.GetRecommendedBooks(userId.Value)).ToList();
                        if (recommendedBooks.Count >= pageSize)
                            books = books.Where(x => recommendedBooks.Select(r => r.BookId).Contains(x.Id));
                        else
                        {
                            var booksNeeded = pageSize - recommendedBooks.Count;
                            var recommendedByFavorites = await _recommendationUtilities.GetRecommendedBooksBasedOnFavorites(userId.Value, booksNeeded);
                            if (recommendedBooks.Count == 0)
                                books = books.Where(x => recommendedByFavorites.Contains(x.Id));
                            else
                                books = books.Where(x => recommendedBooks.Select(r => r.BookId).Contains(x.Id) || recommendedByFavorites.Contains(x.Id));
                        }
                    }
                    else
                    {
                        books = books.OrderByDescending(x => x.Orders.Count());
                    }
                    break;

                case "new":
                    books = books.OrderByDescending(x => x.PublishDate);
                    break;
                case "hot":
                    books = books.OrderByDescending(x => x.Orders.Count());
                    break;

                default:
                    break;
            }

            return books.Take(pageSize);
        }

        private async Task<IQueryable<Book>> SetupMainBooksQuery(IEnumerable<int>? authors, IEnumerable<int>? publishers, int? minYear, int? maxYear, int? categoryId, IEnumerable<string> searchItems, string type, int? userId)
        {
            var books = _db.Books.AsQueryable();

            if (minYear.HasValue)
                books = books.Where(x => x.PublishDate.Value.Year >= minYear.Value);

            if (maxYear.HasValue)
                books = books.Where(x => x.PublishDate.Value.Year <= maxYear.Value);

            if (authors != null && authors.Count() > 0)
                books = books.Where(x => x.Authors.Any(a => authors.Contains(a.Id)));

            if (publishers != null && publishers.Count() > 0)
                books = books.Where(x => x.Publishers.Any(p => publishers.Contains(p.Id)));

            if (categoryId.HasValue)
                books = books.Where(x => x.Categories.Any(c => c.Id == categoryId.Value));

            if (searchItems.Count() > 0)
            {
                var predicate = PredicateBuilder.New<Book>();
                foreach (var searchItem in searchItems)
                    predicate = predicate.Or(x => x.Title.ToLower().Contains(searchItem.ToLower()));

                books = books.AsExpandableEFCore().Where(predicate);
            }

            switch (type)
            {
                case "new":
                    books = books.OrderByDescending(x => x.PublishDate);
                    break;
                case "hot":
                    books = books.OrderByDescending(x => x.Orders.Count());
                    break;

                default:
                    break;
            }

            return books;
        }

        private async Task<List<BookResponse>> OrderMainRecommendedBooks(List<BookResponse> books, int? userId)
        {
            if (!userId.HasValue)
                return books;

            var recommendedBooks = (await _recommendationUtilities.GetRecommendedBooks(userId.Value)).ToList();
            var userFavoriteCategories = await _userRepository.GetFavoriteCategories(userId.Value);
            var userFavoriteAuthors = await _userRepository.GetFavoriteAuthors(userId.Value);
            return books.OrderFoundBooksBasedOnRecommendations(recommendedBooks, userFavoriteCategories, userFavoriteAuthors);
        }

        public async Task<IEnumerable<BookResponse>> GetBooksByFilters(IEnumerable<int>? authors, IEnumerable<int>? publishers, int? minYear, int? maxYear, int? categoryId, IEnumerable<string> searchItems, string type, int? userId)
        {
            var forHome = type.Contains("-home");
            var sortType = type.Split('-')[0];

            var books = forHome ? await SetupHomeBooksQuery(userId, sortType) : await SetupMainBooksQuery(authors, publishers, minYear, maxYear, categoryId, searchItems, type, userId);

            var foundBooks = await books.Select(x =>  new BookResponse
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

            if (!forHome)
                foundBooks = foundBooks.OrderFoundBooks(searchItems);

            if (!forHome && (type == "recommended" || type == ""))
                foundBooks = await OrderMainRecommendedBooks(foundBooks, userId);

            return foundBooks;
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
