using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class RuleNameController : ControllerBase
    {
        private readonly IRuleNameService _ruleNameService;

        public RuleNameController(IRuleNameService ruleNameService)
        {
            _ruleNameService = ruleNameService;
        }

        // GET : All RuleName
        [HttpGet("List")]
        public async Task<IActionResult> GetAllRuleName()
        {
            IEnumerable<RuleName> ruleNames = await _ruleNameService.GetAllRuleName();
            return Ok(ruleNames);
        }

        // GET : RuleName by ID
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> GetRuleNameById(int id)
        {
            RuleName? ruleName = await _ruleNameService.GetRuleNameById(id);
            if (ruleName == null)
                return NotFound(new { message = RuleNameModelRessources.RuleNameNotFound });

            return Ok(ruleName);
        }

        // POST : New RuleName
        [HttpPost("Creation")]
        public async Task<IActionResult> AddRuleName([FromBody] RuleNameModel ruleNameModel)
        {
            var result = await _ruleNameService.AddRuleName(ruleNameModel);
            if (result.Errors.Any())
                return BadRequest(result.Errors);
            return Ok(result.Data);
        }

        // GET : RuleNameModel by ID for UpdateForm
        [HttpGet("FormUpdate/{id}")]
        public async Task<IActionResult> ShowUpdateForm(int id)
        {
            RuleNameModel? ruleNameModel = await _ruleNameService.GetRuleNameModelById(id);
            if (ruleNameModel == null)
                return NotFound(new { message = RuleNameModelRessources.RuleNameNotFound });
            return Ok(ruleNameModel);
        }

        // PUT : Update RuleName with RuleNameModel
        [HttpPut("Modification/{id}")]
        public async Task<IActionResult> UpdateRating(int id, [FromBody] RuleNameModel ruleNameModel)
        {
            var result = await _ruleNameService.UpdateRuleName(ruleNameModel, id);
            if (result.Errors.Any())
                return BadRequest(result.Errors);
            return Ok(result.Data);
        }

        // DELETE : Delete RuleName by ID
        [HttpDelete("Removal/{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            bool success = await _ruleNameService.DeleteRuleName(id);
            if (!success)
                return NotFound(new { message = RuleNameModelRessources.RuleNameNotFound });
            return Ok();
        }
    }
}