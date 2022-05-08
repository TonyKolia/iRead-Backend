using iRead.API.Repositories;
using iRead.API.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iRead.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : CustomControllerBase
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorController(IAuthorRepository _authorRepository, ILogger<CustomControllerBase> logger):base(logger)
        {
            this._authorRepository = _authorRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> Get()
        {
            var authors = await _authorRepository.GetAuthors();
            return ReturnIfNotEmpty(authors, "No authors found.");
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Author>> Get(int id)
        {
            var author = await _authorRepository.GetAuthor(id);
            return ReturnIfNotEmpty(author, $"Author with id {id} not found");
        }
    }
}
