using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Controllers;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Repositories.Interfaces;

namespace P7CreateRestApi.Repositories
{
    public class RuleNameRepository : IRuleNameRepository
    {
        private readonly LocalDbContext _DbContext;

        public RuleNameRepository(LocalDbContext localDbContext)
        {
            _DbContext = localDbContext;
        }

        public async Task<IEnumerable<RuleName>> GetAllRuleName()
        {
            return await _DbContext.RuleNames.ToListAsync();
        }

        public async Task<RuleName?> GetRuleNameById(int id)
        {
            return await _DbContext.RuleNames.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddRuleName(RuleName ruleName)
        {
            _DbContext.RuleNames.Add(ruleName);
            await _DbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateRuleName(RuleName ruleName)
        {
            RuleName? findRuleName = await GetRuleNameById(ruleName.Id);
            if (findRuleName == null)
                return false;

            _DbContext.Entry(findRuleName).CurrentValues.SetValues(ruleName);
            await _DbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRuleName(int id)
        {
            RuleName? findRuleName = await GetRuleNameById(id);
            if (findRuleName == null)
                return false;

            _DbContext.RuleNames.Remove(findRuleName);
            await _DbContext.SaveChangesAsync();
            return true;
        }

    }
}
