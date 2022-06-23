using iRead.API.Models.Email;

namespace iRead.API.Utilities.Interfaces
{
    public interface IEmailUtilities
    {
        public Task SendEmail(EmailType emailType, int? userId = null, int? orderId = null, string email = null);
    }
}
