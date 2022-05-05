using iRead.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace iRead.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentificationController : CustomControllerBase
    {
        private readonly IIdentificationRepository _identificationRepository;

        public IdentificationController(IIdentificationRepository _identificationRepository, ILogger<CustomControllerBase> logger) : base(logger)
        {
            this._identificationRepository = _identificationRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentificationMethod>>> Get()
        {
            return Ok(await _identificationRepository.GetIdentificationMethods());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Gender>> Get(int id)
        {
            return Ok(await _identificationRepository.GetIdentificationMethod(id));
        }
    }
}
