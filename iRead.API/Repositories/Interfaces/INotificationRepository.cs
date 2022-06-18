namespace iRead.API.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        public Task<UserNotification> GetNotification(int id);
        public Task<IEnumerable<UserNotification>> GetNotViewedNotifications(int userId);
        public Task GenerateAndSaveNotifications(int userId);
        public Task<IEnumerable<UserNotification>> GetNotifications(int userId);
        public Task<int> GetNumberOfUnreadNotifications(int userId);
        public Task<UserNotification> CreateNotification(UserNotification notification);
        public Task<IEnumerable<UserNotification>> CreateUserNotifications(IEnumerable<UserNotification> notifications);
        public Task<UserNotification> UpdateNotification(UserNotification notification); 
    }
}
