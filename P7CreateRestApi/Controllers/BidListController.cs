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
    public class BidListController : ControllerBase
    {

        private readonly IBidListService _BidListService;

        public BidListController(IBidListService bidListService)
        {
            _BidListService = bidListService;
        }

        // GET : All BidList
        [HttpGet("List")]
        public async Task<IActionResult> GetAllBidList()
        {
            IEnumerable<BidList> bidLists = await _BidListService.GetAllBidList();
            return Ok(bidLists);
        }

        // GET : BidList by ID
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> GetBidListById(int id)
        {
            BidList? bidlist = await _BidListService.GetBidListById(id);
            if (bidlist == null)
                return NotFound(new { message = BidListModelRessources.BidListNotFound });

            return Ok(bidlist);
        }

        // POST : New Bidlist
        [HttpPost("Creation")]
        public async Task<IActionResult> AddNewBidList([FromBody] BidListModel bidList)
        {
            string username = User.Identity!.Name!;
            var result = await _BidListService.AddBidList(bidList, username);
            if (result.Errors.Any())
                return BadRequest(result.Errors);
            return Ok(result.Data);
        }

        // GET : BidListModel by ID for UpdateForm
        [HttpGet("FormUpdate/{id}")]
        public async Task<IActionResult> ShowUpdateForm(int id)
        {
            BidListModel? bidListModel = await _BidListService.GetBidListModelById(id);
            if (bidListModel == null)
                return NotFound(new { message = BidListModelRessources.BidListNotFound });
            return Ok(bidListModel);
        }

        // PUT : Update BidList with BidListModel
        [HttpPut("Modification/{id}")]
        public async Task<IActionResult> UpdateBidList(int id, [FromBody] BidListModel bidListModel)
        {
            string username = User.Identity!.Name!;
            var result = await _BidListService.UpdateBidList(bidListModel, username, id);
            if (result.Errors.Any())
                return BadRequest(result.Errors);
            return Ok(result.Data);
        }

        // DELETE : Delete Bidlist by ID
        [HttpDelete("Removal/{id}")]
        public async Task<IActionResult> DeleteBidList(int id)
        {
            List<ValidationResult> errors = await _BidListService.DeleteBidList(id);
            if (errors.Any())
                return NotFound(errors);
            return Ok();
        }
    }
}