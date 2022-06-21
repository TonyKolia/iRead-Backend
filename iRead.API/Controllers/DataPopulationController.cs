using iRead.API.Repositories;
using iRead.API.Repositories.Interfaces;
using iRead.API.Utilities.Interfaces;
using iRead.API.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using iRead.API.Models.Favorite;
using iRead.API.Models.Order;
using iRead.API.Models.Rating;
using Microsoft.EntityFrameworkCore;

namespace iRead.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataPopulationController : ControllerBase
    {
        private readonly iReadDBContext _db;
        private readonly IAuthenticationUtilities _authenticationUtilities;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGenderRepository _genderRepository;
        private readonly IIdentificationRepository _identificationRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IRatingRepository _ratingRepository;

        public DataPopulationController(iReadDBContext _db, IRatingRepository _ratingRepository, IOrderRepository _orderRepository, IFavoriteRepository _favoriteRepository, IIdentificationRepository _identificationRepository, IGenderRepository _genderRepository,  IUserRepository _userRepository, IAuthenticationUtilities _authenticationUtilities, IAuthorRepository _authorRepository, ICategoryRepository _categoryRepository)
        {
            this._db = _db;
            this._authorRepository = _authorRepository;
            this._categoryRepository = _categoryRepository;
            this._userRepository = _userRepository;
            this._genderRepository = _genderRepository;
            this._identificationRepository = _identificationRepository;
            this._favoriteRepository = _favoriteRepository;
            this._orderRepository = _orderRepository;
            this._ratingRepository = _ratingRepository;
            this._authenticationUtilities = _authenticationUtilities;
        }


        [HttpPost]
        [Route("Generate/{count}")]
        public async Task<ActionResult> Generate(int count)
        {
            try
            {
                var authors = await _authorRepository.GetAuthors();
                var categories = await _categoryRepository.GetCategories();
                var genders = await _genderRepository.GetGenders();
                var identificationMethods = await _identificationRepository.GetIdentificationMethods();

                var maxId = _db.Users.Max(x => x.Id) + 1;

                for (var i = maxId; i < maxId + count; i++)
                {
                    await GenerateUserFull(i, authors, categories, genders, identificationMethods);
                }

                return Ok($"Generated and added {count} new users");
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        private async Task<int> GenerateAndCreateUser(int id, IEnumerable<int> favoriteAuthors, IEnumerable<int> favoriteCategories, int gender, int identificationMethod)
        {
            string salt = string.Empty;
            var authors = (await _authorRepository.GetMultipleAuthors(favoriteAuthors)) as List<Author>;
            var categories = (await _categoryRepository.GetMultipleCategories(favoriteCategories)) as List<Category>;
            var accountData = new User
            {
                Username = $"user{id}",
                Password = _authenticationUtilities.HashPassword("123", out salt),
                Salt = salt,
                RegisterDate = DateTime.Now,
                LastLogin = DateTime.Now,
                UserCategory = 1,
                Active = 1,
                Authors = authors,
                Categories = categories
            };

            var createdUser = await _userRepository.CreateUser(accountData);

            accountData.MemberContactInfo = new MemberContactInfo
            {
                Address = $"Address{id} {id}",
                City = $"City{id}",
                PostalCode = "12345",
                Telephone = "2101234567",
                Email = $"user{id}@gmail.com"
            };

            accountData.MemberPersonalInfo = new MemberPersonalInfo
            {
                Name = string.Format("Name{0}", id),
                Surname = $"Surname{id}",
                Birthdate = new DateTime(1990, 10, 10),
                Gender = gender,
                IdType = identificationMethod,
                IdNumber = Guid.NewGuid().ToString()
            };

            await _userRepository.UpdateUser(accountData);

            return createdUser.Id;
        }

        private async Task GenerateAndCreateFavoriteBooks(int userId, IEnumerable<int> favoriteCategories)
        {
            var random = new Random();
            var numberOfFavoriteBooksFromOrders = random.Next(15, 21);

            var booksFromOrders = _db.Books.Where(x => x.Orders.Count(o => o.UserId == userId) > 0).OrderRandomly().Take(numberOfFavoriteBooksFromOrders).Select(x => new NewFavorite { BookId = x.Id, UserId = userId }).ToList();
            var idsToExclude = booksFromOrders.Select(x => x.BookId);

            var numberOfFavoriteBooksFromFavoriteCategories = random.Next(1, 6);
            var numberOfFavoriteBooksFromOtherCategories = random.Next(1, 6);

            var booksFromFavoriteCategories = _db.Books.Where(x => !idsToExclude.Contains(x.Id) && x.Categories.Any(c => favoriteCategories.Contains(c.Id))).OrderRandomly().Take(numberOfFavoriteBooksFromFavoriteCategories).Select(x => new NewFavorite { BookId = x.Id, UserId = userId }).ToList();
            var booksFromOtherCategories = _db.Books.Where(x => !idsToExclude.Contains(x.Id) && x.Categories.Any(c => !favoriteCategories.Contains(c.Id))).OrderRandomly().Take(numberOfFavoriteBooksFromFavoriteCategories).Select(x => new NewFavorite { BookId = x.Id, UserId = userId }).ToList();

            var favoritesToAdd = new List<NewFavorite>();
            favoritesToAdd.AddRange(booksFromOrders);
            favoritesToAdd.AddRange(booksFromFavoriteCategories);
            favoritesToAdd.AddRange(booksFromOtherCategories);

            foreach (var favoriteToAdd in favoritesToAdd)
                await _favoriteRepository.CreateFavorite(favoriteToAdd);
        }

        private async Task GenerateAndCreateOrders(int userId, IEnumerable<int> favoriteCategories)
        {
            var totalBooks = 30; //30 / 3 = 10 orders per user
            var random = new Random();
            var numberOfBooksFromFavoriteCategories = random.Next(18, 23);
            var numberOfBooksFromOtherCategories = totalBooks - numberOfBooksFromFavoriteCategories;

            var booksFromFavoriteCategories = _db.Books.Where(x => x.Categories.Any(c => favoriteCategories.Contains(c.Id))).OrderRandomly().Take(numberOfBooksFromFavoriteCategories).Select(x => x.Id).ToList();
            var booksFromOtherCategories = _db.Books.Where(x => x.Categories.Any(c => !favoriteCategories.Contains(c.Id))).OrderRandomly().Take(numberOfBooksFromOtherCategories).Select(x => x.Id).ToList();

            var booksToOrder = new List<int>();
            booksToOrder.AddRange(booksFromFavoriteCategories);
            booksToOrder.AddRange(booksFromOtherCategories);
            booksToOrder = booksToOrder.RandomlyOrderList().ToList();
            
            while(booksToOrder.Count > 0)
            {
                await _orderRepository.CreateOrder(new NewOrder { UserId = userId, Books = booksToOrder.Take(3).ToList() });
                booksToOrder.RemoveRange(0, 3);
            }
        }

        private async Task GenerateAndCreateRatings(int userId)
        {
            var random = new Random();
            var orderedBooks = await _db.Books.Where(x => x.Orders.Count(o => o.UserId == userId) > 0).Select(x => x.Id).ToListAsync();
            foreach (var orderedBook in orderedBooks)
                await _ratingRepository.CreateRating(new NewRating { BookId = orderedBook, Comment = "", UserId = userId, Rating = random.Next(1, 6) });
        }

        private async Task GenerateUserFull(int id, IEnumerable<Author> authors, IEnumerable<Category> categories, IEnumerable<Gender> genders, IEnumerable<IdentificationMethod> identificationMethods)
        {
            var favoriteCategories = categories.RandomlyOrderList().Take(3).Select(x => x.Id);
            var favoriteAuthors = authors.RandomlyOrderList().Take(3).Select(x => x.Id);

            var random = new Random();
            var randomId = random.Next(1, 3);
            var gender = genders.FirstOrDefault(x => x.Id == randomId).Id;
            randomId = random.Next(1, 4);
            var idType = identificationMethods.FirstOrDefault(x => x.Id == randomId).Id;

            var userId = await GenerateAndCreateUser(id, favoriteAuthors, favoriteCategories, gender, idType);

            //orders
            await GenerateAndCreateOrders(userId, favoriteCategories);

            //ratings
            await GenerateAndCreateRatings(userId);

            //favorites
            await GenerateAndCreateFavoriteBooks(userId, favoriteCategories);
        }

    }
}
