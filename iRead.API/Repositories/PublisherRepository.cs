using iRead.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace iRead.API.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly iReadDBContext _db;

        public PublisherRepository(iReadDBContext _db)
        {
            this._db = _db;
        }

        public async Task<Publisher> GetPublisher(int id)
        {
            return await _db.Publishers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Publisher>> GetPublishers()
        {
            return await _db.Publishers.OrderBy(x => x.Name).ToListAsync();
        }
    }
}
