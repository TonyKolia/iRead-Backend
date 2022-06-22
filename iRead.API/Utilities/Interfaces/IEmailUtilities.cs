using iRead.API.Models.Email;

namespace iRead.API.Utilities.Interfaces
{
    public interface IEmailUtilities
    {
        public Task SendEmail(EmailData email);
        public Task<EmailData> GenerateOrderConfirmationEmail(int userId, int orderId);
    }
}
