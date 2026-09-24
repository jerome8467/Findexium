using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Ressource;
using P7CreateRestApi.Service.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Service
{
    public class RuleNameService: IRuleNameService
    {
        private readonly IRuleNameRepository _ruleNameRepository;

        public RuleNameService(IRuleNameRepository ruleNameRepository)
        {
            _ruleNameRepository = ruleNameRepository;
        }

        public async Task<IEnumerable<RuleName>> GetAllRuleName()
        {
            return await _ruleNameRepository.GetAllRuleName();
        }

        public async Task<RuleName?> GetRuleNameById(int id)
        {
            return await GetRuleNameById(id);
        }

        public async Task<RuleNameModel?> GetRuleNameModelById(int id)
        {
            RuleName? ruleName = await GetRuleNameById(id);
            if (ruleName == null)
                return null;
            return MappingRuleNameForModel(ruleName);
        }

        public async Task<ServiceResult<RuleName>> AddRuleName(RuleNameModel ruleNameModel)
        {
            var result = new ServiceResult<RuleName>();
            ValidationContext context = new ValidationContext(ruleNameModel);
            
            if(!Validator.TryValidateObject(ruleNameModel, context, result.Errors, true))
                return result;

            RuleName ruleName = MappingRuleNameModelForDatabase(ruleNameModel);
            await _ruleNameRepository.AddRuleName(ruleName);
            result.Data = ruleName;
            return result;
        }

        public async Task<ServiceResult<RuleName>> UpdateRuleName(RuleNameModel ruleNameModel, int id)
        {
            ValidationContext context = new ValidationContext(ruleNameModel);
            var result = new ServiceResult<RuleName>();

            if (!Validator.TryValidateObject(ruleNameModel, context, result.Errors, true))
                return result;

            RuleName ruleName = MappingRuleNameModelForDatabase(ruleNameModel);
            ruleName.Id = id;

            bool success = await _ruleNameRepository.UpdateRuleName(ruleName);
            if (!success)
            {
                result.Errors.Add(new ValidationResult(RuleNameModelRessources.RuleNameNotFound));
                return result;
            }

            result.Data = ruleName;
            return result;

        }

        public async Task<bool> DeleteRuleName(int id)
        {
            return await _ruleNameRepository.DeleteRuleName(id);
        }

        private RuleName MappingRuleNameModelForDatabase(RuleNameModel ruleNameModel)
        {
            RuleName ruleName = new RuleName
            {
                Name = ruleNameModel.Name,
                Description = ruleNameModel.Description,
                Json = ruleNameModel.Json,
                Template = ruleNameModel.Template,
                SqlStr = ruleNameModel.SqlStr,
                SqlPart = ruleNameModel.SqlPart,
            };

            return ruleName;
        }

        private RuleNameModel MappingRuleNameForModel(RuleName ruleName)
        {
            RuleNameModel ruleNameModel = new RuleNameModel
            {
                Name = ruleName.Name,
                Description = ruleName.Description,
                Json = ruleName.Json,
                Template = ruleName.Template,
                SqlStr = ruleName.SqlStr,
                SqlPart = ruleName.SqlPart,
            };

            return ruleNameModel;
        }


    }
}
