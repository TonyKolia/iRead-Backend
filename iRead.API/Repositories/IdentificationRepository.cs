using Microsoft.EntityFrameworkCore;

namespace iRead.API.Repositories
{
    public class IdentificationRepository : IIdentificationRepository
    {
        private readonly iReadDBContext _db;

        public IdentificationRepository(iReadDBContext db)
        {
            this._db = db;
        }

        public async Task<IdentificationMethod> GetIdentificationMethod(int id)
        {
            return await _db.IdentificationMethods.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<IdentificationMethod>> GetIdentificationMethods()
        {
            return await _db.IdentificationMethods.ToListAsync();
        }
    }
}
