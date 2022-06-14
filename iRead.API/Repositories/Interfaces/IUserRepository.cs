namespace iRead.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUser(int id);
        Task<User> GetUser(string username);
        Task<User> CreateUser(User user);
        Task<User> UpdateUser(User user);
        Task<bool> UserExists(int id);
        Task<bool> UserExists(string username);
        Task<IEnumerable<int>> GetFavoriteCategories(int id);
        Task<IEnumerable<int>> GetFavoriteAuthors(int id);
        Task<IEnumerable<User>> GetUsersByFavoriteCategory(int categoryId);
    }
}
