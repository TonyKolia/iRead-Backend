using Microsoft.EntityFrameworkCore;

namespace iRead.API.Repositories
{
    public class GenderRepository : IGenderRepository
    {
        private readonly iReadDBContext _db;

        public GenderRepository(iReadDBContext db)
        {
            this._db = db;
        }

        public async Task<Gender> GetGender(int id)
        {
            return await _db.Genders.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Gender>> GetGenders()
        {
            return await _db.Genders.ToListAsync();
        }
    }
}
