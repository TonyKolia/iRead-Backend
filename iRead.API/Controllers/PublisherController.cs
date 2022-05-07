using iRead.API.Models;
using iRead.API.Repositories.Interfaces;
using iRead.API.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iRead.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : CustomControllerBase
    {
        private readonly IPublisherRepository _publisherRepository;

        public PublisherController(IPublisherRepository _publisherRepository, ILogger<CustomControllerBase> logger) : base(logger)
        {
            this._publisherRepository = _publisherRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublisherResponse>>> Get()
        {
            var publishers = await _publisherRepository.GetPublishers();
            return publishers.Count() > 0 ? Ok(publishers.MapResponse()) : NotFound("No publishers found.");
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<PublisherResponse>> Get(int id)
        {
            var publisher = await _publisherRepository.GetPublisher(id);
            return publisher != null ? Ok(publisher.MapResponse()) : NotFound($"Publisher with id {id} not found.");
        }


    }
}
