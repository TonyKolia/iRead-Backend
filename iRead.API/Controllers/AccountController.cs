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
        private readonly IEncryptionUtilities _encryptionUtilities;
        private readonly IMemberRepository _memberRepository;

        public AccountController(IMemberRepository _memberRepository, IEncryptionUtilities _encryptionUtilities, IUserRepository _userRepository, IValidationUtilities _validationUtilities, ILogger<CustomControllerBase> logger):base(logger)
        {
            this._validationUtilities = _validationUtilities;
            this._userRepository = _userRepository;
            this._encryptionUtilities = _encryptionUtilities;
            this._memberRepository = _memberRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register([FromBody] RegistrationForm form)
        {
            try
            {
                if (form == null || !_validationUtilities.IsObjectCompletelyPopulated(form))
                    return BadRequest("Not all fields have been filled in.");

                if(await _userRepository.UserExists(form.Username))
                    return BadRequest($"User with username {form.Username} already exists");

                var validationResult = _validationUtilities.ValidateRegistrationForm(form);

                if (!validationResult.Success)
                    return BadRequest(validationResult.Messages);

                var accountData = new User
                {
                    Id = 0,
                    Username = form.Username,
                    Password = _encryptionUtilities.EncryptPassword(form.Password),
                    RegisterDate = DateTime.Now,
                    UserCategory = 1,
                    Active = 1
                };

                var createdUser = await _userRepository.CreateUser(accountData);

                var personalData = new MemberPersonalInfo 
                {
                    UserId = createdUser.Id,
                    Name = form.Name,
                    Surname = form.Surname,
                    Birthdate = form.Birthdate.Value,
                    Gender = form.Gender.Value,
                    IdType = form.IdType.Value
                };

                var contactData = new MemberContactInfo 
                {
                    UserId = createdUser.Id,
                    Address = form.Address,
                    City = form.City,
                    PostalCode = form.PostalCode,
                    Telephone = form.Telephone,
                    Email = form.Email
                };

                await _memberRepository.CreateMemberPersonalInfo(personalData);
                await _memberRepository.CreateMemberContactInfo(contactData);

                return StatusCode(StatusCodes.Status201Created, await _memberRepository.GetMemberFullInfo(createdUser.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occured.");
            }

        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login([FromBody] LoginForm form)
        {
            if (form == null || !_validationUtilities.IsObjectCompletelyPopulated(form))
                return BadRequest("Not all fields have been filled in.");

            if (!await _userRepository.UserExists(form.Username))
                return BadRequest("Invalid credential combination.");

            var user = await _userRepository.GetUser(form.Username);

            if (user.Password == _encryptionUtilities.EncryptPassword(form.Password))
                return Ok();
            else
                return BadRequest("Invalid credential combination.");
        }
    }
}
