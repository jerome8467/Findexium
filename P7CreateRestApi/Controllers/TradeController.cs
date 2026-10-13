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
    public class TradeController : ControllerBase
    {
        private readonly ITradeService _tradeService;

        public TradeController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }

        /// <summary>
        /// GET: Returns all trades in DTO format.
        /// </summary>
        [HttpGet("List")]
        public async Task<IActionResult> GetAllTrade()
        {
            IEnumerable<TradeDto> trades = await _tradeService.GetAllTrade();
            return Ok(trades);
        }

        /// <summary>
        /// GET: Returns a trade by id in DTO format.
        /// </summary>
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> GetTradById(int id)
        {
            TradeDto? trade = await _tradeService.GetTradeById(id);
            if(trade == null)
            {
                return NotFound(new { message = TradeModelRessources.TradeNotFound });
            }
            return Ok(trade);
        }

        /// <summary>
        /// POST: Creates a trade from the submitted model and returns it in DTO format.
        /// </summary>
        [HttpPost("Creation")]
        public async Task<IActionResult> AddTrade([FromBody]TradeModel tradeModel)
        {
            string username = User.Identity!.Name!;
            var result = await _tradeService.AddTrade(tradeModel, username);
            if(result.Errors.Any())
                return BadRequest(result.Errors);
            return Ok (result.Data);
        }

        /// <summary>
        /// GET: Returns a trade by id in model format, to prefill the update form.
        /// </summary>
        [HttpGet("FormUpdate/{id}")]
        public async Task<IActionResult> ShowUpdateForm(int id)
        {
            TradeModel? tradeModel = await _tradeService.GetTradeModelById(id);
            if (tradeModel == null)
                return NotFound(new { message = TradeModelRessources.TradeNotFound});
            return Ok(tradeModel);
        }

        /// <summary>
        /// PUT: Updates a trade by id from the submitted model and returns it in DTO format.
        /// </summary>
        [HttpPut("Modification/{id}")]
        public async Task<IActionResult> UpdateTrade(int id, [FromBody] TradeModel tradeModel)
        {
            // Name of the connected user, read from the JWT token
            string username = User.Identity!.Name!;
            var result = await _tradeService.UpdateTrade(tradeModel, username, id);
            if (result.Errors.Any())
                return BadRequest(result.Errors);
            return Ok(result.Data);
        }

        /// <summary>
        /// DELETE: Deletes a trade by id.
        /// </summary>
        [HttpDelete("Removal/{id}")]
        public async Task<IActionResult> DeleteTrade(int id)
        {
            List<ValidationResult> errors = await _tradeService.DeleteTrade(id);
            if(errors.Any())
                return NotFound(errors);
            return Ok();
        }
    }
}