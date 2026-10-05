using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;
using P7CreateRestApi.Ressource;
using P7CreateRestApi.Service;
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

        // GET : All Trade
        [HttpGet("list")]
        public async Task<IActionResult> GetAllTrade()
        {
            IEnumerable<TradeDto> trades = await _tradeService.GetAllTrade();
            return Ok(trades);
        }

        // GET : Trade by ID
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

        // POST : new Trade
        [HttpPost("Creation")]
        public async Task<IActionResult> AddTrade([FromBody]TradeModel tradeModel)
        {
            string username = User.Identity!.Name!;
            var result = await _tradeService.AddTrade(tradeModel, username);
            if(result.Errors.Any())
                return BadRequest(result.Errors);
            return Ok (result.Data);
        }

        // Get : TradeMOdel by ID for UpdateForme
        [HttpGet("FormUpdate/{id}")]
        public async Task<IActionResult> ShowUpdateForm(int id)
        {
            TradeModel? tradeModel = await _tradeService.GetTradeModelById(id);
            if (tradeModel == null)
                return NotFound(new { message = TradeModelRessources.TradeNotFound});
            return Ok(tradeModel);
        }

        // PUT : Update Trade with TradeModel
        [HttpPut("Modification/{id}")]
        public async Task<IActionResult> UpdateTrade(int id, [FromBody] TradeModel tradeModel)
        {
            string username = User.Identity!.Name!;
            var result = await _tradeService.UpdateTrade(tradeModel, username, id);
            if (result.Errors.Any())
                return BadRequest(result.Errors);
            return Ok(result.Data);
        }

        // DELETE : Delete Trade by ID
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