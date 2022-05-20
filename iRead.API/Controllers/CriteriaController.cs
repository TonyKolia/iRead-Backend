using iRead.API.Models;
using iRead.API.Repositories;
using iRead.API.Repositories.Interfaces;
using iRead.API.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iRead.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CriteriaController : CustomControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPublisherRepository _publisherRepository;
        private readonly IBookRepository _bookRepository;

        public CriteriaController(IBookRepository _bookRepository, IAuthorRepository _authorRepository, ICategoryRepository _categoryRepository, IPublisherRepository _publisherRepository,  ILogger<CustomControllerBase> logger) : base(logger)
        {
            this._authorRepository = _authorRepository;
            this._categoryRepository = _categoryRepository;
            this._publisherRepository = _publisherRepository;
            this._bookRepository = _bookRepository;
        }

        [HttpGet]
        public async Task<ActionResult<CriteriaResponse>> Get()
        {
            var criteria = new CriteriaResponse
            {
                Authors = (await _authorRepository.GetAuthors()).MapResponse().CastObjectToList<AuthorResponse>(),
                Categories = (await _categoryRepository.GetCategories()).MapResponse().CastObjectToList<CategoryResponse>(),
                Publishers = (await _publisherRepository.GetPublishers()).MapResponse().CastObjectToList<PublisherResponse>(),
                MinYear = await _bookRepository.GetMinPublishYear(),
                MaxYear = await _bookRepository.GetMaxPublishYear()
            };

            return ReturnIfNotEmpty(criteria, "No criteria found", false);
        }
    }
}
