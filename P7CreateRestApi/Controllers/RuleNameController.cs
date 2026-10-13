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
    public class RuleNameController : ControllerBase
    {
        private readonly IRuleNameService _ruleNameService;

        public RuleNameController(IRuleNameService ruleNameService)
        {
            _ruleNameService = ruleNameService;
        }

        /// <summary>
        /// GET: Returns all rule names in DTO format.
        /// </summary>
        [HttpGet("List")]
        public async Task<IActionResult> GetAllRuleName()
        {
            IEnumerable<RuleNameDto> ruleNames = await _ruleNameService.GetAllRuleName();
            return Ok(ruleNames);
        }

        /// <summary>
        /// GET: Returns a rule name by id in DTO format.
        /// </summary>
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> GetRuleNameById(int id)
        {
            RuleNameDto? ruleName = await _ruleNameService.GetRuleNameById(id);
            if (ruleName == null)
                return NotFound(new { message = RuleNameModelRessources.RuleNameNotFound });

            return Ok(ruleName);
        }

        /// <summary>
        /// POST: Creates a rule name from the submitted model and returns it in DTO format.
        /// </summary>
        [HttpPost("Creation")]
        public async Task<IActionResult> AddRuleName([FromBody] RuleNameModel ruleNameModel)
        {
            var result = await _ruleNameService.AddRuleName(ruleNameModel);
            if (result.Errors.Any())
                return BadRequest(result.Errors);
            return Ok(result.Data);
        }

        /// <summary>
        /// GET: Returns a rule name by id in model format, to prefill the update form.
        /// </summary>
        [HttpGet("FormUpdate/{id}")]
        public async Task<IActionResult> ShowUpdateForm(int id)
        {
            RuleNameModel? ruleNameModel = await _ruleNameService.GetRuleNameModelById(id);
            if (ruleNameModel == null)
                return NotFound(new { message = RuleNameModelRessources.RuleNameNotFound });
            return Ok(ruleNameModel);
        }

        /// <summary>
        /// PUT: Updates a rule name by id from the submitted model and returns it in DTO format.
        /// </summary>
        [HttpPut("Modification/{id}")]
        public async Task<IActionResult> UpdateRuleName(int id, [FromBody] RuleNameModel ruleNameModel)
        {
            var result = await _ruleNameService.UpdateRuleName(ruleNameModel, id);
            if (result.Errors.Any())
                return BadRequest(result.Errors);
            return Ok(result.Data);
        }

        /// <summary>
        /// DELETE: Deletes a rule name by id.
        /// </summary>
        [HttpDelete("Removal/{id}")]
        public async Task<IActionResult> DeleteRuleName(int id)
        {
            List<ValidationResult> errors = await _ruleNameService.DeleteRuleName(id);
            if (errors.Any())
                return NotFound(errors);
            return Ok();
        }
    }
}