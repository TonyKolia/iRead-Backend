using iRead.API.Models;
using iRead.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using iRead.API.Utilities;

namespace iRead.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : CustomControllerBase
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IUserRepository _userRepository;

        public MemberController(IUserRepository _userRepository, IMemberRepository _memberRepository, ILogger<CustomControllerBase> logger) : base(logger)
        {
            this._memberRepository = _memberRepository;
            this._userRepository = _userRepository;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<MemberResponse>> GetFullInfo(int id)
        {
            return await _userRepository.UserExists(id) ? Ok(await _memberRepository.GetMemberFullInfo(id)) : NotFound($"User with id {id} not found.");
        }


        #region Personal Info

        [HttpGet]
        [Route("Personal/{id}")]
        public async Task<ActionResult<MemberPersonalInfoResponse>> GetPersonalInfo(int id)
        {
            var personalInfo = await _memberRepository.GetMemberPersonalInfo(id);
            return personalInfo != null ? Ok(personalInfo.MapResponse()) : NotFound($"Personal information for member with id {id} not found.");
        }

        [HttpPost]
        [Route("Personal")]
        public async Task<ActionResult<MemberPersonalInfoResponse>> CreatePersonalInfo(MemberPersonalInfo personalInfo)
        {
            try
            {
                var createdPersonalInfo = await _memberRepository.CreateMemberPersonalInfo(personalInfo);
                return StatusCode(StatusCodes.Status201Created, createdPersonalInfo.MapResponse());
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occured, please try again.");
            }
        }

        [HttpPut]
        [Route("Personal")]
        public async Task<ActionResult<MemberPersonalInfoResponse>> UpdatePersonalInfo(MemberPersonalInfo personalInfo)
        {
            try
            {
                var updatedPersonalInfo = await _memberRepository.UpdateMemberPersonalInfo(personalInfo);
                return Ok(updatedPersonalInfo.MapResponse());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occured, please try again.");
            }
        }

        #endregion

        #region Contact Info

        [HttpGet]
        [Route("Contact/{id}")]
        public async Task<ActionResult<MemberContactInfoResponse>> GetContactInfo(int id)
        {
            var contactInfo = await _memberRepository.GetMemberContactInfo(id);
            return contactInfo != null ? Ok((contactInfo).MapResponse()) : NotFound($"Contact information for member with id {id} not found.");
        }

        [HttpPost]
        [Route("Contact")]
        public async Task<ActionResult<MemberContactInfoResponse>> CreateContactInfo(MemberContactInfo contactInfo)
        {
            try
            {
                var createdContactInfo = await _memberRepository.CreateMemberContactInfo(contactInfo);
                return StatusCode(StatusCodes.Status201Created, createdContactInfo.MapResponse());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occured, please try again.");
            }
        }

        [HttpPut]
        [Route("Contact")]
        public async Task<ActionResult<MemberContactInfoResponse>> UpdateContactInfo(MemberContactInfo contactInfo)
        {
            try
            {
                var updatedContactInfo = await _memberRepository.UpdateMemberContactInfo(contactInfo);
                return Ok(updatedContactInfo.MapResponse());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occured, please try again.");
            }
        }

        #endregion
    }
}
