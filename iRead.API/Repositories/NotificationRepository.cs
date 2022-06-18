using iRead.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using iRead.API.Utilities;

namespace iRead.API.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly iReadDBContext _db;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBookRepository _bookRepository;

        public NotificationRepository(IBookRepository _bookRepository, IUserRepository _userRepository, IOrderRepository _orderRepository, IFavoriteRepository _favoriteRepository, iReadDBContext _db)
        {
            this._db = _db;
            this._favoriteRepository = _favoriteRepository;
            this._orderRepository = _orderRepository;
            this._userRepository = _userRepository;
            this._bookRepository = _bookRepository;
        }

        public async Task<UserNotification> CreateNotification(UserNotification notification)
        {
            _db.Entry(notification).State = EntityState.Added;
            await _db.SaveChangesAsync();
            return notification;
        }

        public async Task<IEnumerable<UserNotification>> CreateUserNotifications(IEnumerable<UserNotification> notifications)
        {
            if (notifications == null || notifications.Count() == 0)
                return notifications;

            foreach(var notification in notifications)
                _db.Entry(notification).State = EntityState.Added;
            
            await _db.SaveChangesAsync();
            return notifications;
        }

        public async Task<UserNotification> GetNotification(int id)
        {
            return await _db.UserNotifications.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<UserNotification>> GetNotifications(int userId)
        {
            return await _db.UserNotifications.Where(x => x.UserId == userId).OrderByDescending(x => x.DateCreated).ToListAsync();
        }

        public async Task<IEnumerable<UserNotification>> GetNotViewedNotifications(int userId)
        {
            return await _db.UserNotifications.Where(x => x.UserId == userId && x.Viewed == 0).OrderByDescending(x => x.DateCreated).ToListAsync();
        }

        public async Task<int> GetNumberOfUnreadNotifications(int userId)
        {
            return await _db.UserNotifications.Where(x => x.UserId == userId && x.Viewed == 0).CountAsync();
        }

        public async Task<UserNotification> UpdateNotification(UserNotification notification)
        {
            _db.Entry(notification).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return notification;
        }

        private IEnumerable<NotificationText> GetAllNotificationTexts()
        {
            return _db.NotificationTexts.ToList();
        }

        public async Task GenerateAndSaveNotifications(int userId)
        {
            var user = await _userRepository.GetUser(userId);
            if ((DateTime.Now - user.LastLogin).TotalDays < 1)
                return;

            var notificationTexts = GetAllNotificationTexts();
            var notifications = new List<UserNotification>();
            var userFavoriteBooks = await _favoriteRepository.GetUserFavorites(userId);
            if(userFavoriteBooks.Any(x => x.BookRead == false && x.Book.Stock > 0))
            {
                var randomUnreadFavorites = userFavoriteBooks.Where(x => x.BookRead == false && x.Book.Stock > 0).Take(3);
                foreach(var randomUnreadFavorite in randomUnreadFavorites)
                {
                    var notificationText = string.Format(notificationTexts.FirstOrDefault(x => x.Id == (int)NotificationType.FavoriteAvailable).NotificationText1, randomUnreadFavorite.Book.Title);
                    notifications.Add(SetupNewNotification(userId, notificationText));
                }
            }


            //check for new books relative to last login
            if(_db.Books.Any(x => x.DateAdded > user.LastLogin))
            {
                var newBooksByCategories = await _bookRepository.GetNewBooksForUserFavoriteCategories(userId);
                var newBooksByAuthors = await _bookRepository.GetNewBooksForUserFavoriteAuthors(userId);

                //addition in favorite category
                foreach (var book in newBooksByCategories)
                {
                    var notificationText = string.Format(notificationTexts.FirstOrDefault(x => x.Id == (int)NotificationType.AdditionInFavoriteCategory).NotificationText1, book.Categories.FirstOrDefault().Description, book.Title);
                    notifications.Add(SetupNewNotification(userId, notificationText));
                }


                //addition in favorite author
                foreach (var book in newBooksByAuthors)
                {
                    var notificationText = string.Format(notificationTexts.FirstOrDefault(x => x.Id == (int)NotificationType.AdditionInFavoriteAuthor).NotificationText1, book.Authors.FirstOrDefault().Name, book.Title);
                    notifications.Add(SetupNewNotification(userId, notificationText));
                }
            }


            var userActiveOrders = await _orderRepository.GetUserActiveOrders(userId);
            if(userActiveOrders.Count() > 0)
            {
                var expiringOrders = userActiveOrders.Where(x => (x.ReturnDate - DateTime.Now).TotalDays <= 3 && (x.ReturnDate - DateTime.Now).TotalDays > 0);
                foreach(var expiringOrder in expiringOrders)
                {
                    var text = notificationTexts.FirstOrDefault(x => x.Id == (int)NotificationType.OrderAboutToExpire).NotificationText1;
                    var expireDays = Math.Round((expiringOrder.ReturnDate - DateTime.Now).TotalDays, 0);
                    var expireHours = Math.Round((expiringOrder.ReturnDate - DateTime.Now).TotalHours, 0);

                    if (expireDays == 1)
                        text = text.Replace("μέρες", "μέρα");

                    if (expireHours == 1)
                        text = text.Replace("ώρες", "ώρα");

                    var notificationText = string.Format(text, "#" + expiringOrder.Id, expireDays, expireHours);
                    notifications.Add(SetupNewNotification(userId, notificationText));
                }

                 var expiredOrders = userActiveOrders.Where(x => (x.ReturnDate - DateTime.Now).TotalDays < 0);
                foreach(var expiredOrder in expiredOrders)
                {
                    var text = notificationTexts.FirstOrDefault(x => x.Id == (int)NotificationType.OrderExpired).NotificationText1;
                    var expireDays = Math.Abs((expiredOrder.ReturnDate - DateTime.Now).TotalDays);
                    if (expireDays < 1)
                        expireDays = 1;
                    else
                        expireDays = Math.Round(expireDays, 0);

                    if(expireDays == 1)
                        text = text.Replace("ες", "α");

                    var notificationText = string.Format(text, "#" + expiredOrder.Id, Math.Abs(expireDays));
                    notifications.Add(SetupNewNotification(userId, notificationText));
                }
            }

            await CreateUserNotifications(notifications);
        }

        private UserNotification SetupNewNotification(int userId, string text)
        {
            return new UserNotification
            {
                UserId = userId,
                DateCreated = DateTime.Now,
                NotificationText = text,
                Viewed = 0
            };
        }
    }

    internal enum NotificationType
    {
        FavoriteAvailable = 1,
        AdditionInFavoriteCategory = 2,
        AdditionInFavoriteAuthor = 3,
        OrderAboutToExpire = 4,
        OrderExpired = 5
    }
}
