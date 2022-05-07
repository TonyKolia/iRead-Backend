using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using iRead.DBModels.Models;
using iRead.API.Repositories;
using iRead.API.Utilities;

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
            var genders = await _genderRepository.GetGenders();
            return genders.Count() > 0 ? Ok(genders.MapResponse()) : NotFound("No genders found.");
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Gender>> Get(int id)
        {
            var gender = await _genderRepository.GetGender(id);
            return gender != null ? Ok(gender.MapResponse()) : NotFound($"Gender with id {id} not found.");
        }
    }
}
