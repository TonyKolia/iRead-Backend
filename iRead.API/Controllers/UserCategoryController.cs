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
            var userCategories = await _userCategoryRepository.GetUserCategories();
            return ReturnIfNotEmpty(userCategories, "No user categories found.");
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<UserCategory>> Get(int id)
        {
            var userCategory = await _userCategoryRepository.GetUserCategory(id);
            return ReturnIfNotEmpty(userCategory, $"User category with id {id} not found.");
        }
    }
}
