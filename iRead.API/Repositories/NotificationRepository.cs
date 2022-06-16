using iRead.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace iRead.API.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly iReadDBContext _db;

        public NotificationRepository(iReadDBContext _db)
        {
            this._db = _db;
        }

        public async Task<UserNotification> CreateNotification(UserNotification notification)
        {
            _db.Entry(notification).State = EntityState.Added;
            await _db.SaveChangesAsync();
            return notification;
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
    }
}
