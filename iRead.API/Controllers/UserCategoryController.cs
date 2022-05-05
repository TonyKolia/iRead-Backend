using iRead.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iRead.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCategoryController : CustomControllerBase
    {
        private readonly IUserCategoryRepository _userCategoryRepository;

        public UserCategoryController(IUserCategoryRepository _userCategoryRepository, ILogger<CustomControllerBase> logger) : base(logger)
        {
            this._userCategoryRepository = _userCategoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserCategory>>> Get()
        {
            return Ok(await _userCategoryRepository.GetUserCategories());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<UserCategory>> Get(int id)
        {
            return Ok(await _userCategoryRepository.GetUserCategory(id));
        }
    }
}
