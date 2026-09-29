using P7CreateRestApi.Domain;


namespace P7CreateRestApi.Repositories.Interfaces
{
    public interface IRuleNameRepository
    {
        public Task<IEnumerable<RuleName>> GetAllRuleName();
        public Task<RuleName?> GetRuleNameById(int id);
        public Task AddRuleName(RuleName ruleName);
        public Task UpdateRuleName(RuleName ruleName);
        public Task DeleteRuleName(RuleName ruleName);
    }
}
