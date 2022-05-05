namespace iRead.API.Repositories
{
    public interface IIdentificationRepository
    {
        Task<IEnumerable<IdentificationMethod>> GetIdentificationMethods();
        Task<IdentificationMethod> GetIdentificationMethod(int id);
    }
}
