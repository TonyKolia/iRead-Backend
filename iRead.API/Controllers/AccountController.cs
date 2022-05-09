using iRead.API.Models;
using iRead.API.Models.Account;
using iRead.API.Repositories.Interfaces;
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

        public AccountController(IMemberRepository _memberRepository, IAuthenticationUtilities _authenticationUtilities, IUserRepository _userRepository, IValidationUtilities _validationUtilities, ILogger<CustomControllerBase> logger):base(logger)
        {
            this._validationUtilities = _validationUtilities;
            this._userRepository = _userRepository;
            this._authenticationUtilities = _authenticationUtilities;
            this._memberRepository = _memberRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<MemberResponse>> Register([FromBody] RegistrationForm data)
        {
            try
            {
                if (data == null || !_validationUtilities.IsObjectCompletelyPopulated(data))
                    return ReturnResponse(ResponseType.BadRequest, "Not all fields have been filled in.");

                if(await _userRepository.UserExists(data.Username))
                    return ReturnResponse(ResponseType.BadRequest, $"User with username {data.Username} already exists.");

                var validationResult = _validationUtilities.ValidateRegistrationForm(data);

                if (!validationResult.Success)
                    return BadRequest(validationResult.Messages);


                var salt = string.Empty;
                var accountData = new User
                {
                    Username = data.Username,
                    Password = _authenticationUtilities.HashPassword(data.Password, out salt),
                    Salt = salt,
                    RegisterDate = DateTime.Now,
                    LastLogin = DateTime.Now,
                    UserCategory = 1,
                    Active = 1
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
            if (form == null || !_validationUtilities.IsObjectCompletelyPopulated(form))
                return ReturnResponse(ResponseType.BadRequest, "Not all fields have been filled in.");

            if (!await _userRepository.UserExists(form.Username))
                return ReturnResponse(ResponseType.BadRequest, "Invalid credential combination.");

            var user = await _userRepository.GetUser(form.Username);

            if (_authenticationUtilities.VerifyPasswordHash(form.Password, user.Password, user.Salt))
                return ReturnResponse(ResponseType.Token, "", new { token = _authenticationUtilities.GenerateToken(user) });
            else
                return ReturnResponse(ResponseType.BadRequest, "Invalid credential combination.");
        }
    }
}
