using iRead.API.Models.Email;
using iRead.API.Repositories.Interfaces;
using iRead.API.Utilities.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace iRead.API.Utilities
{
    public class EmailUtilities : IEmailUtilities
    {
        private string _AddressFrom { get; set; }
        private string _SMTP { get; set; }
        private int _Port { get; set; }
        private string _Password { get; set; }
        private readonly IConfiguration _config;
        private readonly IMemberRepository _memberRepository;
        private readonly IOrderRepository _orderRepository;

        public EmailUtilities(IOrderRepository _orderRepository, IMemberRepository _memberRepository, IConfiguration _config)
        {
            this._config = _config;
            this._AddressFrom = _config.GetValue<string>("EmailSettings:AddressFrom");
            this._SMTP = _config.GetValue<string>("EmailSettings:SMTP");
            this._Port = _config.GetValue<int>("EmailSettings:Port");
            this._Password = _config.GetValue<string>("EmailSettings:Password");
            this._memberRepository = _memberRepository;
            this._orderRepository = _orderRepository;
        }

        public async Task SendEmail(EmailData emailData)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_AddressFrom));
            email.To.Add(MailboxAddress.Parse(emailData.AddressTo));
            email.Subject = emailData.Subject;
            email.Body = new TextPart(TextFormat.Plain) { Text = emailData.Body };

            using(var smtpClient = new SmtpClient())
            {
                smtpClient.Connect(_SMTP, _Port, SecureSocketOptions.StartTls);
                smtpClient.Authenticate(_AddressFrom, _Password);
                smtpClient.Send(email);
                smtpClient.Disconnect(true);
            }
        }

        public async Task<EmailData> GenerateOrderConfirmationEmail(int userId, int orderId)
        {
            var user = await _memberRepository.GetMemberFullInfo(userId);
            var order = await _orderRepository.GetOrder(orderId);
            var returnDate = $"{order.ReturnDate.Day}/{order.ReturnDate.Month}/{order.ReturnDate.Year}";
            return new EmailData
            {
                AddressTo = user.ContactInfo.Email,
                Subject = $"iRead - Επιβεβαίωση κράτησης #{orderId}",
                Body = $"Γεια σας {user.PersonalInfo.Name},\n\nΗ παραγγελία σας με κωδικό #{orderId} έχει καταχωρηθεί με επιτυχία και είναι έτοιμη για παραλαβή.\nΗ επιστροφή των βιβλίων πρέπει να πραγματοποιηθεί έως και τις {returnDate}.\n\nΜε εκτίμηση,\nΗ ομάδα της βιβλιοθήκης iRead"
            };
        }
    }
}
