namespace iRead.API.Repositories.Interfaces
{
    public interface IEmailTextRepository
    {
        Task<EmailText> GetEmailText(string type);
        Task<EmailText> GetEmailText(int id);
    }
}
