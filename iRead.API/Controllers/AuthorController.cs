using iRead.API.Repositories;
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
            return Ok(await _authorRepository.GetAuthors());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Author>> Get(int id)
        {
            return Ok(await _authorRepository.GetAuthor(id));
        }
    }
}
