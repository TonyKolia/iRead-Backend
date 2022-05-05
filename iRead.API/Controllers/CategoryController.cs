using iRead.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iRead.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : CustomControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository _categoryRepository, ILogger<CustomControllerBase> logger) : base(logger)
        {
            this._categoryRepository = _categoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            return Ok(await _categoryRepository.GetCategories());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Category>> Get(int id)
        {
            return Ok(await _categoryRepository.GetCategory(id));
        }
    }
}
