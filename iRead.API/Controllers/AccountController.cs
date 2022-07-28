using iRead.API.Models;
using iRead.API.Models.Account;
using iRead.API.Repositories;
using iRead.API.Repositories.Interfaces;
using iRead.API.Utilities;
using iRead.API.Utilities.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iRead.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : CustomControllerBase
    {
        private readonly IValidationUtilities _validationUtilities;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationUtilities _authenticationUtilities;
        private readonly IMemberRepository _memberRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IEmailUtilities _emailUtilities;


        public AccountController(IEmailUtilities _emailUtilities, INotificationRepository _notificationRepository, IAuthorRepository _authorRepository, ICategoryRepository _categoryRepository, IMemberRepository _memberRepository, IAuthenticationUtilities _authenticationUtilities, IUserRepository _userRepository, IValidationUtilities _validationUtilities, ILogger<CustomControllerBase> logger):base(logger)
        {
            this._validationUtilities = _validationUtilities;
            this._userRepository = _userRepository;
            this._authenticationUtilities = _authenticationUtilities;
            this._memberRepository = _memberRepository;
            this._authorRepository = _authorRepository;
            this._categoryRepository = _categoryRepository;
            this._notificationRepository = _notificationRepository;
            this._emailUtilities = _emailUtilities;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<MemberResponse>> Register([FromBody] RegistrationForm data)
        {
            try
            {
                var validationResult = await _validationUtilities.ValidateRegistrationForm(data);

                if (!validationResult.Success)
                    return ReturnResponse(ResponseType.BadRequest, "Errors occured during validation", validationResult);

                var salt = string.Empty;
                var accountData = new User
                {
                    Username = data.Username,
                    Password = _authenticationUtilities.HashPassword(data.Password, out salt),
                    Salt = salt,
                    RegisterDate = DateTime.Now,
                    LastLogin = DateTime.Now,
                    UserCategory = 1,
                    Active = 0,
                    Authors = (await _authorRepository.GetMultipleAuthors(data.FavoriteAuthors)) as List<Author>,
                    Categories = (await _categoryRepository.GetMultipleCategories(data.FavoriteCategories)) as List<Category>
                };

                var createdUser = await _userRepository.CreateUser(accountData);

                accountData.MemberContactInfo = new MemberContactInfo
                {
                    Address = data.Address,
                    City = data.City,
                    PostalCode = data.PostalCode,
                    Telephone = data.Telephone,
                    Email = data.Email
                }; 

                accountData.MemberPersonalInfo = new MemberPersonalInfo
                {
                    Name = data.Name,
                    Surname = data.Surname,
                    Birthdate = data.Birthdate.Value,
                    Gender = data.Gender.Value,
                    IdType = data.IdType.Value,
                    IdNumber = data.IdNumber
                };

                await _userRepository.UpdateUser(accountData);
                await _notificationRepository.GenerateAndCreateWelcomeNotification(createdUser.Id);

                return ReturnResponse(ResponseType.Created, "", await _memberRepository.GetMemberFullInfo(createdUser.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ReturnResponse(ResponseType.Error);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login([FromBody] LoginForm form)
        {
            try
            {
                if (form == null || !_validationUtilities.IsObjectCompletelyPopulated(form))
                    return ReturnResponse(ResponseType.BadRequest, "Παρακαλώ συμπληρώστε όλα τα πεδία");

                if (!await _userRepository.UserExists(form.Username))
                    return ReturnResponse(ResponseType.BadRequest, "Λανθασμένος συνδυασμός στοιχείων σύνδεσης.");

                var user = await _userRepository.GetUser(form.Username);

                if (_authenticationUtilities.VerifyPasswordHash(form.Password, user.Password, user.Salt))
                {
                    await _notificationRepository.GenerateAndSaveNotifications(user.Id);
                    var token = _authenticationUtilities.GenerateToken(user);
                    user.ActivationGuid = token;
                    user.LastLogin = DateTime.Now;
                    await _userRepository.UpdateUser(user);
                    if (user.Active == 0)
                    {
                        await _emailUtilities.SendEmail(EmailType.AccountActivation, user.Id);
                        await _notificationRepository.GenerateAndCreateAccountActivationNotification(user.Id);
                    }
                    return ReturnResponse(ResponseType.Token, "", new LoginResponse { Token = token, UserId = user.Id, Username = user.Username });
                }
                else
                    return ReturnResponse(ResponseType.BadRequest, "Λανθασμένος συνδυασμός στοιχείων σύνδεσης.");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return ReturnResponse(ResponseType.Error);
            }
           
        }

        [HttpPost]
        [Route("ActivateAccount/{userId}/{token}")]
        public async Task<ActionResult<bool>> ActivateAccount(int userId, string token)
        {
            try
            {
                var accountActivated = await _userRepository.ActivateAccount(userId, token);
                return ReturnResponse(ResponseType.Updated, "", accountActivated);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return ReturnResponse(ResponseType.Error);
            }
        }

        [HttpPost]
        [Route("SendPasswordResetEmail/{email}")]
        public async Task<ActionResult> SendPasswordResetEmail(string email)
        {
            try
            {
                var validationResult = await _validationUtilities.ValidateEmail(email);
                if(!validationResult.Success)
                    return ReturnResponse(ResponseType.BadRequest, "Errors occured during validation", validationResult);

                await _emailUtilities.SendEmail(EmailType.PasswordReset, email: email);
                return ReturnResponse(ResponseType.Created, "Email sent");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return ReturnResponse(ResponseType.Error);
            }
        }

        [HttpPost]
        [Route("ResetPassword/{userId}/{token}")]
        public async Task<ActionResult> ResetPassword([FromBody] PasswordResetForm passwordResetForm, int userId, string token)
        {
            try
            {
                var user = await _userRepository.GetUser(userId);
                if (user.ActivationGuid != token)
                    return ReturnResponse(ResponseType.BadRequest, "Errors occured during validation", "Η επαναφορά του κωδικού πρόσβασης απέτυχε. Παρακαλούμε δοκιμάστε ξανά.");

                var validationResult = await _validationUtilities.ValidatePasswordChange(userId, passwordResetForm.Password, passwordResetForm.ConfirmPassword);
                if(!validationResult.Success)
                    return ReturnResponse(ResponseType.BadRequest, "Errors occured during validation", validationResult);

                user.Password = _authenticationUtilities.HashPassword(passwordResetForm.Password, out var salt);
                user.Salt = salt;
                await _userRepository.UpdateUser(user);
                return ReturnResponse(ResponseType.Updated, "Password reset successfully");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return ReturnResponse(ResponseType.Error);
            }
        }
    }
}
