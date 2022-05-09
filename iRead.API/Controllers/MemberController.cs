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
            var user = await _memberRepository.GetMemberFullInfo(id);
            return ReturnIfNotEmpty(user, $"User with id {id} not found.", false);
        }


        #region Personal Info

        [HttpGet]
        [Route("Personal/{id}")]
        public async Task<ActionResult<MemberPersonalInfoResponse>> GetPersonalInfo(int id)
        {
            var personalInfo = await _memberRepository.GetMemberPersonalInfo(id);
            return ReturnIfNotEmpty(personalInfo, $"Personal information for member with id {id} not found.");
        }

        [HttpPost]
        [Route("Personal")]
        public async Task<ActionResult<MemberPersonalInfoResponse>> CreatePersonalInfo(MemberPersonalInfo personalInfo)
        {
            try
            {
                var createdPersonalInfo = await _memberRepository.CreateMemberPersonalInfo(personalInfo);
                return ReturnResponse(ResponseType.Created, "", createdPersonalInfo.MapResponse());
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return ReturnResponse(ResponseType.Error);
            }
        }

        [HttpPut]
        [Route("Personal")]
        public async Task<ActionResult<MemberPersonalInfoResponse>> UpdatePersonalInfo(MemberPersonalInfo personalInfo)
        {
            try
            {
                var updatedPersonalInfo = await _memberRepository.UpdateMemberPersonalInfo(personalInfo);
                return ReturnResponse(ResponseType.Updated, "", updatedPersonalInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ReturnResponse(ResponseType.Error);
            }
        }

        #endregion

        #region Contact Info

        [HttpGet]
        [Route("Contact/{id}")]
        public async Task<ActionResult<MemberContactInfoResponse>> GetContactInfo(int id)
        {
            var contactInfo = await _memberRepository.GetMemberContactInfo(id);
            return ReturnIfNotEmpty(contactInfo, $"Contact information for member with id {id} not found.");
        }

        [HttpPost]
        [Route("Contact")]
        public async Task<ActionResult<MemberContactInfoResponse>> CreateContactInfo(MemberContactInfo contactInfo)
        {
            try
            {
                var createdContactInfo = await _memberRepository.CreateMemberContactInfo(contactInfo);
                return ReturnResponse(ResponseType.Created, "", createdContactInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ReturnResponse(ResponseType.Error);
            }
        }

        [HttpPut]
        [Route("Contact")]
        public async Task<ActionResult<MemberContactInfoResponse>> UpdateContactInfo(MemberContactInfo contactInfo)
        {
            try
            {
                var updatedContactInfo = await _memberRepository.UpdateMemberContactInfo(contactInfo);
                return ReturnResponse(ResponseType.Updated, "", updatedContactInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ReturnResponse(ResponseType.Error);
            }
        }

        #endregion
    }
}
