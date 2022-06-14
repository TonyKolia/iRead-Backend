using iRead.RecommendationSystem.Models;
using iRead.API.Utilities.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using iRead.API.Models.Recommendation;
using Microsoft.AspNetCore.Authorization;

namespace iRead.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private readonly IRecommendationUtilities _recommendationUtilities;

        public RecommendationController(IRecommendationUtilities _recommendationUtilities)
        {
            this._recommendationUtilities = _recommendationUtilities;
        }

        [HttpPost]
        [Route("Train")]
        public async Task<ActionResult> TrainModel()
        {
            try
            {
                await _recommendationUtilities.TrainModel();
                return Ok();
            }
            catch(Exception ex)
            {
                return Ok(ex.Message);
            }
        }

       

        #region Testing

        [HttpPost]
        [Route("Predict")]
        public async Task<ActionResult> Predict([FromBody] RecommendationInput input)
        {
            try
            {
                var result = await _recommendationUtilities.MakePrediction(input);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("Predict/{id}")]
        public async Task<ActionResult> Predict(int id)
        {
            try
            {
                var result = await _recommendationUtilities.GetRecommendedBooks(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        #endregion

       
    }
}
