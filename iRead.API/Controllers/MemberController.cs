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
        public async Task<ActionResult<MemberPersonalInfo>> GetPersonalInfo(int id)
        {
            return Ok((await _memberRepository.GetMemberPersonalInfo(id)).MapResponse());
        }

        [HttpPost]
        [Route("Personal")]
        public async Task<ActionResult<MemberPersonalInfo>> CreatePersonalInfo(MemberPersonalInfo personalInfo)
        {
            try
            {
                return StatusCode(StatusCodes.Status201Created, (await _memberRepository.CreateMemberPersonalInfo(personalInfo)).MapResponse());
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occured, please try again.");
            }
        }

        [HttpPut]
        [Route("Personal")]
        public async Task<ActionResult<MemberPersonalInfo>> UpdatePersonalInfo(MemberPersonalInfo personalInfo)
        {
            try
            {
                return StatusCode(StatusCodes.Status201Created, (await _memberRepository.UpdateMemberPersonalInfo(personalInfo)).MapResponse());
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
        public async Task<ActionResult<MemberContactInfo>> GetContactInfo(int id)
        {
            return Ok((await _memberRepository.GetMemberContactInfo(id)).MapResponse());
        }

        [HttpPost]
        [Route("Contact")]
        public async Task<ActionResult<MemberContactInfo>> CreateContactInfo(MemberContactInfo contactInfo)
        {
            try
            {
                return StatusCode(StatusCodes.Status201Created, (await _memberRepository.CreateMemberContactInfo(contactInfo)).MapResponse());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occured, please try again.");
            }
        }

        [HttpPut]
        [Route("Contact")]
        public async Task<ActionResult<MemberContactInfo>> UpdateContactInfo(MemberContactInfo contactInfo)
        {
            try
            {
                return StatusCode(StatusCodes.Status201Created, (await _memberRepository.UpdateMemberContactInfo(contactInfo)).MapResponse());
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
