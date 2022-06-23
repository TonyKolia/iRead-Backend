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
        private readonly IEmailTextRepository _emailTextRepository;
        private readonly IUserRepository _userRepository;

        public EmailUtilities(IUserRepository _userRepository, IEmailTextRepository _emailTextRepository, IOrderRepository _orderRepository, IMemberRepository _memberRepository, IConfiguration _config)
        {
            this._config = _config;
            this._AddressFrom = _config.GetValue<string>("EmailSettings:AddressFrom");
            this._SMTP = _config.GetValue<string>("EmailSettings:SMTP");
            this._Port = _config.GetValue<int>("EmailSettings:Port");
            this._Password = _config.GetValue<string>("EmailSettings:Password");
            this._memberRepository = _memberRepository;
            this._orderRepository = _orderRepository;
            this._emailTextRepository = _emailTextRepository;
            this._userRepository = _userRepository;
        }

        private async Task SendEmail(EmailData emailData)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_AddressFrom));
            email.To.Add(MailboxAddress.Parse(emailData.AddressTo));
            email.Subject = emailData.Subject;
            email.Body = new TextPart(TextFormat.Text) { Text = emailData.Body.Replace("<br>", "\n") };

            using(var smtpClient = new SmtpClient())
            {
                smtpClient.Connect(_SMTP, _Port, SecureSocketOptions.StartTls);
                smtpClient.Authenticate(_AddressFrom, _Password);
                smtpClient.Send(email);
                smtpClient.Disconnect(true);
            }
        }

        private async Task GenerateAndSendOrderConfirmationEmail(int userId, int orderId)
        {
            var emailText = await _emailTextRepository.GetEmailText("OrderConfirmation");
            if (emailText == null)
                return;

            var user = await _memberRepository.GetMemberFullInfo(userId);
            var order = await _orderRepository.GetOrder(orderId);
            var returnDate = $"{order.ReturnDate.Day}/{order.ReturnDate.Month}/{order.ReturnDate.Year}";

            var emailData = new EmailData
            {
                AddressTo = user.ContactInfo.Email,
                Subject = string.Format(emailText.Subject, orderId),
                Body = string.Format(emailText.Body, user.PersonalInfo.Name, orderId, returnDate)
            };

            await SendEmail(emailData);
        }

        private async Task GenerateAndSendAccountActivationEmail(int userId)
        {
            var user = await _userRepository.GetUser(userId);
            if (string.IsNullOrEmpty(user.ActivationGuid))
                return;

            var emailText = await _emailTextRepository.GetEmailText("AccountConfirmation");
            if (emailText == null)
                return;

            var memberData = await _memberRepository.GetMemberFullInfo(userId);
            var clientUrl = _config.GetValue<string>("ClientUrl");
            var activationLink = string.Format(clientUrl+"/AccountActivation/userId/{0}/token/{1}", user.Id, user.ActivationGuid);

            var emailData = new EmailData
            {
                AddressTo = memberData.ContactInfo.Email,
                Subject = string.Format(emailText.Subject),
                Body = string.Format(emailText.Body, memberData.PersonalInfo.Name, activationLink)
            };

            await SendEmail(emailData);
        }

        private async Task GenerateAndSendPasswordResetEmail(string email)
        {
            var emailText = await _emailTextRepository.GetEmailText("PasswordReset");
            if (emailText == null)
                return;

            var user = await _memberRepository.GetMemberContactInfo(email);
            if (user == null)
                return;

            var token = (await _userRepository.GetUser(user.UserId)).ActivationGuid;
            var clientUrl = _config.GetValue<string>("ClientUrl");
            var activationLink = string.Format(clientUrl+"/passwordReset/userId/{0}/token/{1}", user.UserId, token);

            var emailData = new EmailData
            {
                AddressTo = email,
                Subject = string.Format(emailText.Subject),
                Body = string.Format(emailText.Body, activationLink)
            };

            await SendEmail(emailData);
        }

        public async Task SendEmail(EmailType emailType, int? userId = null, int? orderId = null, string email = null)
        {
            switch (emailType) 
            {
                case EmailType.OrderConfirmation:
                    await GenerateAndSendOrderConfirmationEmail(userId.Value, orderId.Value);
                    break;
                case EmailType.AccountActivation:
                    await GenerateAndSendAccountActivationEmail(userId.Value);
                    break;
                case EmailType.PasswordReset:
                    await GenerateAndSendPasswordResetEmail(email);
                    break;
                default:
                    break;
            }
        }
    }

    public enum EmailType 
    {
        OrderConfirmation,
        AccountActivation,
        PasswordReset
    }

}
