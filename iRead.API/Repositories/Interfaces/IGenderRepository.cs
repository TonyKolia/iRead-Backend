using iRead.DBModels.Models;

namespace iRead.API.Repositories
{
    public interface IGenderRepository
    {
       Task<IEnumerable<Gender>> GetGenders();
       Task<Gender> GetGender(int id);
    }
}