namespace iRead.API.Repositories.Interfaces
{
    public interface IUserCategoryRepository
    {
        Task<IEnumerable<UserCategory>> GetUserCategories();
        Task<UserCategory> GetUserCategory(int id);
    }
}
