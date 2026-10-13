using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;
using P7CreateRestApi.Ressource;
using P7CreateRestApi.Service.Interfaces;
using System.ComponentModel.DataAnnotations;

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

        /// <summary>
        /// GET: Returns all ratings in DTO format.
        /// </summary>
        [HttpGet("List")]
        public async Task<IActionResult> GetAllRating()
        {
            IEnumerable<RatingDto> rating = await _ratingService.GetAllRating();
            return Ok(rating);
        }

        /// <summary>
        /// GET: Returns a rating by id in DTO format.
        /// </summary>
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> GetRatingById(int id)
        {
            RatingDto? rating = await _ratingService.GetRatingById(id);
            if (rating == null)
                return NotFound(new { message = RatingModelRessources.RatingNotFound });

            return Ok(rating);
        }

        /// <summary>
        /// POST: Creates a rating from the submitted model and returns it in DTO format.
        /// </summary>
        [HttpPost("Creation")]
        public async Task<IActionResult> AddRating([FromBody] RatingModel ratingModel)
        {
            var result = await _ratingService.AddRating(ratingModel);
            if (result.Errors.Any())
                return BadRequest(result.Errors);
            return Ok(result.Data);
        }

        /// <summary>
        /// GET: Returns a rating by id in model format, to prefill the update form.
        /// </summary>
        [HttpGet("FormUpdate/{id}")]
        public async Task<IActionResult> ShowUpdateForm(int id)
        {
            RatingModel? ratingModel = await _ratingService.GetRatingModelById(id);
            if (ratingModel == null)
                return NotFound(new { message = RatingModelRessources.RatingNotFound });
            return Ok(ratingModel);
        }

        /// <summary>
        /// PUT: Updates a rating by id from the submitted model and returns it in DTO format.
        /// </summary>
        [HttpPut("Modification/{id}")]
        public async Task<IActionResult> UpdateRating(int id, [FromBody] RatingModel ratingModel)
        {
            var result = await _ratingService.UpdateRating(ratingModel, id);
            if (result.Errors.Any())
                return BadRequest(result.Errors);
            return Ok(result.Data);
        }

        /// <summary>
        /// DELETE: Deletes a rating by id.
        /// </summary>
        [HttpDelete("Removal/{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            List<ValidationResult> errors = await _ratingService.DeleteRating(id);
            if (errors.Any())
                return NotFound(errors);
            return Ok();
        }
    }
}