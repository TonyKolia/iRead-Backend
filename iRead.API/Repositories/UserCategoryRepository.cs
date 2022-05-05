using iRead.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace iRead.API.Repositories
{
    public class UserCategoryRepository : IUserCategoryRepository
    {
        private readonly iReadDBContext _db;

        public UserCategoryRepository(iReadDBContext db)
        {
            this._db = db;
        }

        public async Task<IEnumerable<UserCategory>> GetUserCategories()
        {
            return await _db.UserCategories.ToListAsync();
        }

        public async Task<UserCategory> GetUserCategory(int id)
        {
            return await _db.UserCategories.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
