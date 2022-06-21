using iRead.API.Models;
using iRead.API.Repositories.Interfaces;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using iRead.API.Utilities;
using iRead.API.Utilities.Interfaces;
using iRead.API.Models.Recommendation;

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

        private async Task<IQueryable<Book>> SetupHomeBooksQuery(int? userId, string type, int pageSize = 6, IEnumerable<int> excludedIds = null)
        {
            var books = _db.Books.AsQueryable();

            switch (type)
            {
                case "recommended":
                    if (userId.HasValue)
                    {
                        var recommendedBooks = (await _recommendationUtilities.GetRecommendedBooks(userId.Value)).ToList();

                        if (excludedIds != null && excludedIds.Count() > 0)
                            recommendedBooks.RemoveAll(x => excludedIds.Contains(x.BookId));

                        if (recommendedBooks.Count >= pageSize)
                            books = books.Where(x => recommendedBooks.Select(r => r.BookId).Contains(x.Id));
                        else
                        {
                            var booksNeeded = pageSize - recommendedBooks.Count;
                            var recommendedByFavorites = await _recommendationUtilities.GetRecommendedBooksBasedOnFavorites(userId.Value, recommendedBooks.Select(x => x.BookId), booksNeeded, excludedIds);
                            if (recommendedBooks.Count == 0)
                                books = books.Where(x => recommendedByFavorites.Contains(x.Id));
                            else
                                books = books.Where(x => recommendedBooks.Select(r => r.BookId).Contains(x.Id) || recommendedByFavorites.Contains(x.Id));
                        }
                    }
                    else
                        books = books.OrderRandomly();
                    break;

                case "new":
                    books = books.OrderByDescending(x => x.DateAdded);
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
                    books = books.OrderByDescending(x => x.DateAdded);
                    break;
                case "hot":
                    books = books.OrderByDescending(x => x.Orders.Count());
                    break;

                default:
                    break;
            }

            return books;
        }

        private async Task<List<BookResponse>> OrderMainRecommendedBooks(List<BookResponse> books, int? userId, bool fromSearch = false)
        {
            if (!userId.HasValue)
                return books.RandomlyOrderList().ToList();

            var recommendedBooks = (await _recommendationUtilities.GetRecommendedBooks(userId.Value)).ToList();
            var userFavoriteCategories = await _userRepository.GetFavoriteCategories(userId.Value);
            var userFavoriteAuthors = await _userRepository.GetFavoriteAuthors(userId.Value);
            return books.OrderFoundBooksBasedOnRecommendations(recommendedBooks, userFavoriteCategories, userFavoriteAuthors, fromSearch);
        }

        public async Task<IEnumerable<BookResponse>> GetBooksByFilters(IEnumerable<int>? authors, IEnumerable<int>? publishers, int? minYear, int? maxYear, int? categoryId, IEnumerable<string> searchItems, string type, int? userId)
        {
            //var forHome = type.Contains("-home");
            //var sortType = type.Split('-')[0];

            //var books = forHome ? await SetupHomeBooksQuery(userId, sortType) : await SetupMainBooksQuery(authors, publishers, minYear, maxYear, categoryId, searchItems, type, userId);
            var books = await GetBookResults(await SetupMainBooksQuery(authors, publishers, minYear, maxYear, categoryId, searchItems, type, userId));
            //var foundBooks = await books.Select(x =>  new BookResponse
            //{
            //    Id = x.Id,
            //    Title = x.Title,
            //    ISBN = x.Isbn,
            //    PageCount = x.PageCount,
            //    Description = x.Description,
            //    ImagePath = x.ImagePath ?? "",
            //    PublishDate = x.PublishDate.Value,
            //    Stock = x.BooksStock.Stock,
            //    Authors = x.Authors.Select(a => new AuthorResponse
            //    {
            //        Id = a.Id,
            //        Name = a.Name,
            //        Surname = a.Surname,
            //        Birthdate = a.Birthdate
            //    }),
            //    Categories = x.Categories.Select(c => new CategoryResponse
            //    {
            //        Id = c.Id,
            //        Description = c.Description ?? ""
            //    }),
            //    Ratings = x.Ratings.Select(r => new RatingResponse
            //    {
            //        Username = r.User.Username,
            //        Rating = r.Rating1,
            //        Comment = r.Comment ?? "",
            //        DateAdded = r.DateAdded
            //    }),
            //    Publishers = x.Publishers.Select(p => new PublisherResponse
            //    {
            //        Id = p.Id,
            //        Name = p.Name,
            //        Description = p.Description ?? ""
            //    })
            
            //}).ToListAsync();

            //if (!forHome)
                books = books.ToList().OrderFoundBooks(searchItems);

            if (type == "recommended" || type == "")
                books = await OrderMainRecommendedBooks(books.ToList(), userId, searchItems.Count() > 0);

            return books;
        }

        public async Task<HomeBooksResponse> GetHomeBooks(int? userId)
        {
            return new HomeBooksResponse
            {
                Recommended = await GetBookResults(await SetupHomeBooksQuery(userId, "recommended", 6)),
                New = await GetBookResults(await SetupHomeBooksQuery(userId, "new", 6)),
                Hot = await GetBookResults(await SetupHomeBooksQuery(userId, "hot", 6))
            };
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

        private async Task<IEnumerable<BookResponse>> GetBookResults(IQueryable<Book> query)
        {
            return await query.Select(x => new BookResponse
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

        private async Task<IEnumerable<BookResponse>> GetOtherUserRecommendations(int bookId, int pageSize, int? userId = null, IEnumerable<int> idsToExclude = null)
        {
            var bookIds = new List<int>();
            //get the category of the book
            var category = _db.Books.Where(x => x.Id == bookId).Select(x => x.Categories.Select(c => c.Id).FirstOrDefault()).FirstOrDefault();

            if(userId.HasValue)
            {
                var userOrders = _db.Books.Where(x => x.Orders.Any(o => o.UserId == userId.Value)).Select(x => x.Id).AsEnumerable();
                if (idsToExclude == null)
                    idsToExclude = userOrders;
                else
                    (idsToExclude as List<int>).AddRange(userOrders);
            }

            //get the users that have selected this category as a favorite
            var users = (await _userRepository.GetUsersByFavoriteCategory(category)).ToList();

            //if the active user is found, exclude him to avoid displaying his lendings
            if (userId.HasValue && users.Any(x => x.Id == userId.Value))
                users = users.Where(x => x.Id != userId.Value).ToList();

            var userIds = users.Select(x => x.Id);

            var query = _db.Books.Where(x => x.Categories.Any(c => c.Id == category) && x.Id != bookId);
            if (idsToExclude != null && idsToExclude.Count() > 0)
                query = query.Where(x => !idsToExclude.Contains(x.Id));

            //if no users found with this favorite category
            if (userIds.Count() == 0)
            {
                if (userId.HasValue) //if active user, exclude his orders
                    query = query.Where(x => x.Orders.Any(o => o.UserId != userId.Value));
                else //if not active users, show all orders
                    query = query.Where(x => x.Orders.Count > 0);
            }

            //if we have users with this favorite category, get their orders
            if (userIds.Count() > 0)
                query = query.Where(x => x.Orders.Any(o => userIds.Contains(o.UserId.Value)));

            var books = await GetBookResults(query.OrderRandomly().Take(pageSize));

            if(books.Count() < pageSize)
            {
                if (idsToExclude == null)
                    idsToExclude = books.Select(x => x.Id);
                else
                    (idsToExclude as List<int>).AddRange(books.Select(x => x.Id));

                var randomBooks = _db.Books.Where(x => x.Id != bookId && !idsToExclude.Contains(x.Id) && x.Categories.Any(c => c.Id == category)).OrderRandomly().Take(pageSize - books.Count());
                (books as List<BookResponse>).AddRange(await GetBookResults(randomBooks));
            }

            return books;
        }

        private async Task<IEnumerable<BookResponse>> GetSimilarBooks (int bookId, int pageSize, IEnumerable<int> idsToExclude = null)
        {
            var book = await GetBook(bookId);
            var bookCategories = book.Categories.Select(x => x.Id);
            var bookAuthors = book.Authors.Select(x => x.Id);
            var books = _db.Books.Where(x => x.Id != bookId && x.Categories.Any(c => bookCategories.Contains(c.Id)) && x.Authors.Any(a => bookAuthors.Contains(a.Id)));

            if (idsToExclude != null && idsToExclude.Count() > 0)
                books = books.Where(x => !idsToExclude.Contains(x.Id));

            var bookResults = (await GetBookResults(books.OrderRandomly().Take(pageSize))).ToList();

            if(bookResults.Count < pageSize)
            {
                if (bookResults.Count == 0)
                {
                    books = _db.Books.Where(x => x.Id != bookId && x.Categories.Any(c => bookCategories.Contains(c.Id)));
                    if (idsToExclude != null && idsToExclude.Count() > 0)
                        books = books.Where(x => !idsToExclude.Contains(x.Id));
                }
                else
                {
                    //exclude found books to avoid duplicates
                    if (idsToExclude != null)
                        (idsToExclude as List<int>).AddRange(bookResults.Select(x => x.Id));
                    else
                        idsToExclude = bookResults.Select(x => x.Id);
                    books = _db.Books.Where(x => x.Id != bookId && x.Categories.Any(c => bookCategories.Contains(c.Id)) && !idsToExclude.Contains(x.Id));
                }

                bookResults.AddRange(await GetBookResults(books.OrderRandomly().Take(pageSize - bookResults.Count)));
            }

            return bookResults;
        }

        private async Task<IEnumerable<BookResponse>> GetTopSellers(int bookId, int pageSize)
        {
            var book = await GetBook(bookId);
            var bookCategories = book.Categories.Select(x => x.Id);

            var query = _db.Books.Where(x => x.Categories.Any(c => bookCategories.Contains(c.Id)) && x.Id != bookId && x.Orders.Count > 0).OrderByDescending(x => x.Orders.Count).Take(pageSize);
            var books = await GetBookResults(query);

            if(books.Count() < pageSize)
            {
                if(books.Count() == 0)
                {
                    query = _db.Books.Where(x => x.Categories.Any(c => bookCategories.Contains(c.Id)) && x.Id != bookId).OrderRandomly().Take(pageSize);
                    books = await GetBookResults(query);
                }
                else
                {
                    var idsToExclude = books.Select(x => x.Id);
                    query = _db.Books.Where(x => x.Categories.Any(c => bookCategories.Contains(c.Id)) && x.Id != bookId && !idsToExclude.Contains(x.Id)).OrderRandomly().Take(pageSize - books.Count());
                    (books as List<BookResponse>).AddRange(await GetBookResults(query));
                }
            }

            return await GetBookResults(query);
        }

        public async Task<RelatedBookRecommendations> GetRecommendedByUserAndBook(int bookId, int userId)
        {
            var pageSize = 4;
            var idsToExclude = new List<int>();
            var userRecommendations = await GetBookResults(await SetupHomeBooksQuery(userId, "recommended", pageSize, new List<int> { bookId }));
            idsToExclude.AddRange(userRecommendations.Select(x => x.Id));
            var otherUsersRecommendations = await GetOtherUserRecommendations(bookId, pageSize, userId, idsToExclude);
            idsToExclude.AddRange(otherUsersRecommendations.Select(x => x.Id));
            var similarRecommendations = await GetSimilarBooks(bookId, pageSize, idsToExclude);
            return new RelatedBookRecommendations(userRecommendations, otherUsersRecommendations, similarRecommendations);
        }

        public async Task<RelatedBookRecommendations> GetRecommendedByBook(int bookId)
        {
            var pageSize = 4;
            var idsToExclude = new List<int>();
            var userRecommendations = await GetTopSellers(bookId, pageSize);
            idsToExclude.AddRange(userRecommendations.Select(x => x.Id));
            var otherUsersRecommendations = await GetOtherUserRecommendations(bookId, pageSize, null, idsToExclude);
            idsToExclude.AddRange(otherUsersRecommendations.Select(x => x.Id));
            var similarRecommendations = await GetSimilarBooks(bookId, pageSize, idsToExclude);
            return new RelatedBookRecommendations(userRecommendations, otherUsersRecommendations, similarRecommendations);
        }

        public async Task<IEnumerable<BookResponse>> GetNewBooksForUserFavoriteCategories(int userId)
        {
            var userLastLogin = (await _userRepository.GetUser(userId)).LastLogin;
            var favoriteCategories = await _userRepository.GetFavoriteCategories(userId);
            var query = _db.Books.Where(x => x.DateAdded > userLastLogin && x.Categories.Any(c => favoriteCategories.Contains(c.Id)));
            return await GetBookResults(query);
        }

        public async Task<IEnumerable<BookResponse>> GetNewBooksForUserFavoriteAuthors(int userId)
        {
            var userLastLogin = (await _userRepository.GetUser(userId)).LastLogin;
            var favoriteAuthors = await _userRepository.GetFavoriteAuthors(userId);
            var query = _db.Books.Where(x => x.DateAdded > userLastLogin && x.Authors.Any(c => favoriteAuthors.Contains(c.Id)));
            return await GetBookResults(query);
        }
    }
}
