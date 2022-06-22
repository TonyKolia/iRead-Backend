using iRead.API.Models.Email;

namespace iRead.API.Utilities.Interfaces
{
    public interface IEmailUtilities
    {
        public Task SendEmail(EmailType emailType, int userId, int? orderId = null);
    }
}
