using iRead.API.Models;
using iRead.API.Models.Rating;
using iRead.API.Repositories.Interfaces;
using iRead.API.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iRead.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : CustomControllerBase
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingController(IRatingRepository _ratingRepository, ILogger<CustomControllerBase> logger) : base(logger)
        {
            this._ratingRepository = _ratingRepository;
        }
        
        
        [HttpPost]
        public async Task<ActionResult<RatingResponse>> Create([FromBody] NewRating rating)
        {
            try
            {
                var createdRating = await _ratingRepository.CreateRating(rating);
                return ReturnResponse(ResponseType.Created, "", createdRating.MapResponse());
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return ReturnResponse(ResponseType.Error);
            }
        }
    }
}
