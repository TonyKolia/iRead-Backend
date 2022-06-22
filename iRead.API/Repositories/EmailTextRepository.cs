using iRead.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace iRead.API.Repositories
{
    public class EmailTextRepository : IEmailTextRepository
    {
        private readonly iReadDBContext _db;

        public EmailTextRepository(iReadDBContext _db)
        {
            this._db = _db;
        }

        public async Task<EmailText> GetEmailText(string type)
        {
            return await _db.EmailTexts.FirstOrDefaultAsync(x => x.Type == type);
        }

        public async Task<EmailText> GetEmailText(int id)
        {
            return await _db.EmailTexts.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
