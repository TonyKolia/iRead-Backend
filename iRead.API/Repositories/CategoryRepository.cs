using iRead.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace iRead.API.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly iReadDBContext _db;

        public CategoryRepository(iReadDBContext db)
        {
            this._db = db;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _db.Categories.OrderBy(x => x.Description).ToListAsync();
        }

        public async Task<Category> GetCategory(int id)
        {
            return await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
