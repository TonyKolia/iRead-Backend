using iRead.API.Models;
using iRead.API.Repositories;
using iRead.API.Utilities;
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
        public async Task<ActionResult<IEnumerable<IdentificationMethodResponse>>> Get()
        {
            var identifications = await _identificationRepository.GetIdentificationMethods();
            return identifications.Count() > 0 ?  Ok(identifications.MapResponse()) : NotFound($"No identification methods found.");
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IdentificationMethodResponse>> Get(int id)
        {
            var identificationMethod = await _identificationRepository.GetIdentificationMethod(id);
            return ReturnIfNotEmpty(identificationMethod, $"Identification method with id {id} not found.");
        }
    }
}
