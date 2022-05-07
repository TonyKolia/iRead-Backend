using iRead.API.Repositories.Interfaces;
using iRead.API.Utilities;
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
            var categories = await _categoryRepository.GetCategories();
            return categories.Count() > 0 ? Ok(categories.MapResponse()) : NotFound("No categories found.");
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Category>> Get(int id)
        {
            var category = await _categoryRepository.GetCategory(id);
            return category != null ? Ok(category.MapResponse()) : NotFound($"Category with id {id} not found.");
        }
    }
}
