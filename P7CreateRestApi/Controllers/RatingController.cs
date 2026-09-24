using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Controllers;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;
using P7CreateRestApi.Ressource;
using P7CreateRestApi.Service;
using P7CreateRestApi.Service.Interfaces;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        // GET : All Rating
        [HttpGet("List")]
        public async Task<IActionResult> GetAllRating()
        {
            IEnumerable<Rating> rating = await _ratingService.GetAllRating();
            return Ok(rating);
        }

        // GET : Rating by ID
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> GetRatingById(int id)
        {
            Rating? rating = await _ratingService.GetRatingById(id);
            if (rating == null)
                return NotFound(new { message = RatingModelRessources.RatingNotFound });

            return Ok(rating);
        }

        // POST : New Rating
        [HttpPost("Creation")]
        public async Task<IActionResult> AddRating([FromBody] RatingModel ratingModel)
        {
            var result = await _ratingService.AddRating(ratingModel);
            if (result.Errors.Any())
                return BadRequest(result.Errors);
            return Ok(result.Data);
        }

        // GET : RatingModel by ID for UpdateForm
        [HttpGet("FormUpdate/{id}")]
        public async Task<IActionResult> ShowUpdateForm(int id)
        {
            RatingModel? ratingModel = await _ratingService.GetRatingModelById(id);
            if (ratingModel == null)
                return NotFound(new { message = RatingModelRessources.RatingNotFound });
            return Ok(ratingModel);
        }

        // PUT : Update Rating with RatingModel
        [HttpPut("Modification/{id}")]
        public async Task<IActionResult> UpdateRating(int id, [FromBody] RatingModel ratingModel)
        {
            var result = await _ratingService.UpdateRating(ratingModel, id);
            if (result.Errors.Any())
                return BadRequest(result.Errors);
            return Ok(result.Data);
        }

        // DELETE : Delete Rating by ID
        [HttpDelete("Removal/{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            bool success = await _ratingService.DeleteRating(id);
            if (!success)
                return NotFound(new { message = RatingModelRessources.RatingNotFound });
            return Ok();
        }
    }
}