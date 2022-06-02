using Microsoft.EntityFrameworkCore;

namespace iRead.API.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly iReadDBContext _db;

        public AuthorRepository(iReadDBContext db)
        {
            this._db = db;
        }

        public async Task<Author> GetAuthor(int id)
        {
            return await _db.Authors.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Author>> GetAuthors()
        {
            return await _db.Authors.OrderBy(x => x.Name).ToListAsync();
        }
    }
}
