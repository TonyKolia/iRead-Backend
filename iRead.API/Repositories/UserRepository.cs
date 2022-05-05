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
    }
}
