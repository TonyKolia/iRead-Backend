using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using iRead.DBModels.Models;
using iRead.API.Repositories;

namespace iRead.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenderController : CustomControllerBase
    {
        private readonly IGenderRepository _genderRepository;

        public GenderController(IGenderRepository _genderRepository, ILogger<CustomControllerBase> logger) : base(logger)
        {
            this._genderRepository = _genderRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gender>>> Get()
        {
            return Ok(await _genderRepository.GetGenders());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Gender>> Get(int id)
        {
            return Ok(await _genderRepository.GetGender(id));
        }
    }
}
