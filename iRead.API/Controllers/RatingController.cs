using iRead.API.Models;
using iRead.API.Models.Rating;
using iRead.API.Repositories.Interfaces;
using iRead.API.Utilities;
using Microsoft.AspNetCore.Authorization;
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
        
        [HttpGet]
        [Route("Book/{bookId}")]
        public async Task<ActionResult<BookRatingsResponse>> GetBookRatings(int bookId)
        {
            var ratings = await _ratingRepository.GetBookRatings(bookId);
            return ReturnIfNotEmpty(ratings, "No ratings found.", false);
        }
        
        [HttpPost]
        [Authorize]
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

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<RatingResponse>> Update([FromBody] NewRating rating)
        {
            if (await _ratingRepository.GetRating(rating.UserId, rating.BookId) == null)
                return ReturnResponse(ResponseType.NotFound, "Rating not found");

            try
            {
                var updatedRating = await _ratingRepository.UpdateRating(rating);
                return ReturnResponse(ResponseType.Updated, "", updatedRating);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return ReturnResponse(ResponseType.Error);
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("User/{userId}/Book/{bookId}")]
        public async Task<ActionResult> Delete(int userId, int bookId)
        {
            if (await _ratingRepository.GetRating(userId, bookId) == null)
                return ReturnResponse(ResponseType.NotFound, "Rating not found");

            try
            {
                await _ratingRepository.DeleteRating(userId, bookId);
                return ReturnResponse(ResponseType.Deleted, "Deleted with success.");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return ReturnResponse(ResponseType.Error);
            }

        }
    }
}
