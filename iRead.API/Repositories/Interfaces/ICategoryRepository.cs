namespace iRead.API.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategory(int id);
        Task<IEnumerable<Category>> GetCategories();
        Task<IEnumerable<Category>> GetMultipleCategories(IEnumerable<int> ids);
    }
}
