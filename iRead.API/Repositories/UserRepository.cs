using iRead.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace iRead.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly iReadDBContext _db;

        public UserRepository(iReadDBContext db)
        {
            this._db = db;
        }

        public async Task<bool> UserExists(int id)
        {
            return await _db.Users.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> UserExists(string username)
        {
            return await _db.Users.AnyAsync(x => x.Username == username);
        }

        public async Task<User> GetUser(int id)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> GetUser(string username)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<User> CreateUser(User user)
        {
            _db.Entry(user).State = EntityState.Added;
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUser(User user)
        {
            if(_db.Users.Any(x => x.Id == user.Id))
            {
                _db.Entry(user).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return user;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<int>> GetFavoriteCategories(int id)
        {
            return await _db.Users.Where(x => x.Id == id).Select(x => x.Categories.Select(c => c.Id).AsEnumerable()).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<int>> GetFavoriteAuthors(int id)
        {
            return await _db.Users.Where(x => x.Id == id).Select(x => x.Authors.Select(c => c.Id).AsEnumerable()).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByFavoriteCategory(int categoryId)
        {
            return await _db.Users.Where(x => x.Categories.Any(c => c.Id == categoryId)).ToListAsync();
        }

        public async Task<bool> UserActive(int id)
        {
            return (await _db.Users.FirstOrDefaultAsync(x => x.Id == id)).Active == 1;
        }

        public async Task<bool> ActivateAccount(int id, string token)
        {
            if (!_db.Users.Any(x => x.Id == id && x.ActivationGuid == token))
                return false;

            var user = await GetUser(id);
            if (user == null)
                return false;

            user.Active = 1;
            _db.Entry(user).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
