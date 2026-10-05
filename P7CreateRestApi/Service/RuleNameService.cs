using Microsoft.AspNetCore.Mvc;
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

        public async Task<IEnumerable<RuleNameDto>> GetAllRuleName()
        {
            IEnumerable<RuleName> ruleNames = await _ruleNameRepository.GetAllRuleName();
            List<RuleNameDto> ruleNameDtos = new List<RuleNameDto>();
            foreach(var dto in ruleNames)
            {
                ruleNameDtos.Add(MappingRuleNameForDto(dto));
            }
            return ruleNameDtos;
        }

        public async Task<RuleNameDto?> GetRuleNameById(int id)
        {
            RuleName? ruleName = await _ruleNameRepository.GetRuleNameById(id);
            if (ruleName == null)
                return null;
            return MappingRuleNameForDto(ruleName);
        }

        public async Task<RuleNameModel?> GetRuleNameModelById(int id)
        {
            RuleName? ruleName = await _ruleNameRepository.GetRuleNameById(id);
            if (ruleName == null)
                return null;
            return MappingRuleNameForModel(ruleName);
        }

        public async Task<ServiceResult<RuleNameDto>> AddRuleName(RuleNameModel ruleNameModel)
        {
            var result = new ServiceResult<RuleNameDto>();
            ValidationContext context = new ValidationContext(ruleNameModel);
            
            if(!Validator.TryValidateObject(ruleNameModel, context, result.Errors, true))
                return result;

            RuleName ruleName = MappingRuleNameModelForDatabase(ruleNameModel, new RuleName());
            await _ruleNameRepository.AddRuleName(ruleName);
            result.Data = MappingRuleNameForDto(ruleName);
            return result;
        }

        public async Task<ServiceResult<RuleNameDto>> UpdateRuleName(RuleNameModel ruleNameModel, int id)
        {
            ValidationContext context = new ValidationContext(ruleNameModel);
            var result = new ServiceResult<RuleNameDto>();

            if (!Validator.TryValidateObject(ruleNameModel, context, result.Errors, true))
                return result;

            RuleName? findRuleName = await _ruleNameRepository.GetRuleNameById(id);
            if(findRuleName == null)
            {
                result.Errors.Add(new ValidationResult(RuleNameModelRessources.RuleNameNotFound));
                return result;
            }

            findRuleName = MappingRuleNameModelForDatabase(ruleNameModel, findRuleName);

            await _ruleNameRepository.UpdateRuleName(findRuleName);

            result.Data = MappingRuleNameForDto(findRuleName);
            return result;
        }

        public async Task<List<ValidationResult>> DeleteRuleName(int id)
        {
            List<ValidationResult> result = new List<ValidationResult>();

            RuleName? findRuleName = await _ruleNameRepository.GetRuleNameById(id);
            if (findRuleName == null)
            {
                result.Add(new ValidationResult(RuleNameModelRessources.RuleNameNotFound));
                return result;
            }
            await _ruleNameRepository.DeleteRuleName(findRuleName);
            return result;
        }

        private RuleName MappingRuleNameModelForDatabase(RuleNameModel ruleNameModel, RuleName ruleName)
        {
            ruleName.Name = ruleNameModel.Name;
            ruleName.Description = ruleNameModel.Description;
            ruleName.Json = ruleNameModel.Json;
            ruleName.Template = ruleNameModel.Template;
            ruleName.SqlStr = ruleNameModel.SqlStr;
            ruleName.SqlPart = ruleNameModel.SqlPart;

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

        private RuleNameDto MappingRuleNameForDto(RuleName ruleName)
        {
            RuleNameDto ruleNameDto = new RuleNameDto
            {
                Id = ruleName.Id,
                Name = ruleName.Name,
                Description = ruleName.Description,
                Json = ruleName.Json,
                Template = ruleName.Template,
                SqlStr = ruleName.SqlStr,
                SqlPart = ruleName.SqlPart,
            };
            return ruleNameDto;
        }

    }
}
