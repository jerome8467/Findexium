using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;
using P7CreateRestApi.Ressource;
using P7CreateRestApi.Service.Interfaces;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CurveController : ControllerBase
    {
        private readonly ICurvePointService _curvePointService;

        public CurveController(ICurvePointService curvePointService)
        {
            _curvePointService = curvePointService;
        }


        // GET : All CurvePoint
        [HttpGet("List")]
        public async Task<IActionResult> GetAllCurvePoint()
        {
            IEnumerable<CurvePoint> curvePoint = await _curvePointService.GetAllCurvePoint();
            return Ok(curvePoint);
        }

        // GET : CurvePoint by ID
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> GetCurvePointById(int id)
        {
            CurvePoint? curvePoint = await _curvePointService.GetCurvePointById(id);
            if (curvePoint == null)
                return NotFound(new { message = CurvePointModelRessources.CurvePointNotFound });

            return Ok(curvePoint);
        }

        // POST : New CurvePoint
        [HttpPost("Creation")]
        public async Task<IActionResult> AddCurvePoint([FromBody] CurvePointModel curvePointModel)
        {
            var result = await _curvePointService.AddCurvePoint(curvePointModel);
            if (result.Errors.Any())
                return BadRequest(result.Errors);
            return Ok(result.Data);
        }

        // GET : CurvePointModel by ID for UpdateForm
        [HttpGet("FormUpdate/{id}")]
        public async Task<IActionResult> ShowUpdateForm(int id)
        {
            CurvePointModel? curvePointModel = await _curvePointService.GetCurvePointModelById(id);
            if (curvePointModel == null)
                return NotFound(new { message = CurvePointModelRessources.CurvePointNotFound });
            return Ok(curvePointModel);
        }

        // PUT : Update CurvePoint with CurvePointModel
        [HttpPut("Modification/{id}")]
        public async Task<IActionResult> UpdateCurvePoint(int id, [FromBody] CurvePointModel curvePointModel)
        {
            var result = await _curvePointService.UpdateCurvePoint(curvePointModel, id);
            if (result.Errors.Any())
                return BadRequest(result.Errors);
            return Ok(result.Data);
        }

        // DELETE : Delete CurvePoint by ID
        [HttpDelete("Removal/{id}")]
        public async Task<IActionResult> DeleteCurvePoint(int id)
        {
            bool success = await _curvePointService.DeleteCurvePoint(id);
            if (!success)
                return NotFound(new { message = CurvePointModelRessources.CurvePointNotFound });
            return Ok();
        }
    }
}