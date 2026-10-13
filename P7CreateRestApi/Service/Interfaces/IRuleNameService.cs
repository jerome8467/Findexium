using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Service.Interfaces
{
    public interface IRuleNameService
    {
        public Task<IEnumerable<RuleNameDto>> GetAllRuleName();
        public Task<RuleNameDto?> GetRuleNameById(int id);
        public Task<RuleNameModel?> GetRuleNameModelById(int id);
        public Task<ServiceResult<RuleNameDto>> AddRuleName(RuleNameModel ruleNameModel);
        public Task<ServiceResult<RuleNameDto>> UpdateRuleName(RuleNameModel ruleNameModel, int id);
        public Task<List<ValidationResult>> DeleteRuleName(int id);
    }
}
