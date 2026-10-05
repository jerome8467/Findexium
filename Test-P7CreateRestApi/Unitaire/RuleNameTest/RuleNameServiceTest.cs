using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Service;
using P7CreateRestApi.Service.Interfaces;
using Xunit;

namespace Test_P7CreateRestApi.Unitaire.RuleNameTest
{
    public class RuleNameServiceTest : IDisposable
    {

        private readonly IRuleNameRepository _ruleNameRepository;
        private readonly IRuleNameService _ruleNameService;
        private LocalDbContext _dbContext;

        public RuleNameServiceTest()
        {
            var options = new DbContextOptionsBuilder<LocalDbContext>()
                .UseSqlite("Data source=:memory:")
                .Options;
            _dbContext = new LocalDbContext(options);
            _dbContext.Database.OpenConnection();
            _dbContext.Database.EnsureCreated();

            _dbContext.Add(CreateValidRuleName(1, "Name1", "Description1", "Json1", "Template1", "SqlStr1", "SqlPart1"));
            _dbContext.Add(CreateValidRuleName(2, "Name2", "Description2", "Json2", "Template2", "SqlStr2", "SqlPart2"));
            _dbContext.Add(CreateValidRuleName(3, "Name3", "Description3", "Json3", "Template3", "SqlStr3", "SqlPart3"));
            _dbContext.Add(CreateValidRuleName(4, "Name4", "Description4", "Json4", "Template4", "SqlStr4", "SqlPart4"));
            _dbContext.SaveChanges();

            _ruleNameRepository = new RuleNameRepository(_dbContext);
            _ruleNameService = new RuleNameService(_ruleNameRepository);
        }

        public RuleName CreateValidRuleName(int id, string Name, string Description, string Json, string Template, string SqlStr, string SqlPart)
        {
            return new RuleName
            {
                Id = id,
                Name = Name,
                Description = Description,
                Json = Json,
                Template = Template,
                SqlStr = SqlStr,
                SqlPart = SqlPart
            };
        }

        public void Dispose()
        {
            _dbContext.Database.CloseConnection();
            _dbContext.Dispose();
        }

        [Fact]
        public async Task Service_GetAllRuleName()
        {
            // ACT
            IEnumerable<RuleNameDto> ruleNames = await _ruleNameService.GetAllRuleName();

            // ASSERT
            Assert.Equal(4, ruleNames.Count());
        }

        [Fact]
        public async Task Service_GetRuleNameById()
        {
            // ACT
            RuleNameDto? ruleName = await _ruleNameService.GetRuleNameById(1);

            // ASSERT
            Assert.NotNull(ruleName);
            Assert.Equal(1, ruleName?.Id);
            Assert.Equal("Name1", ruleName?.Name);
            Assert.Equal("Description1", ruleName?.Description);
            Assert.Equal("Json1", ruleName?.Json);
            Assert.Equal("Template1", ruleName?.Template);
            Assert.Equal("SqlStr1", ruleName?.SqlStr);
            Assert.Equal("SqlPart1", ruleName?.SqlPart);
        }

        [Fact]
        public async Task Service_GetRuleNameModelById()
        {
            // ACT
            RuleNameModel? ruleNameModel = await _ruleNameService.GetRuleNameModelById(2);

            // ASSERT
            Assert.NotNull(ruleNameModel);
            Assert.Equal("Name2", ruleNameModel?.Name);
            Assert.Equal("Description2", ruleNameModel?.Description);
            Assert.Equal("Json2", ruleNameModel?.Json);
            Assert.Equal("Template2", ruleNameModel?.Template);
            Assert.Equal("SqlStr2", ruleNameModel?.SqlStr);
            Assert.Equal("SqlPart2", ruleNameModel?.SqlPart);
        }

        [Fact]
        public async Task Service_UpdateRuleName()
        {
            // ACT
            RuleNameModel ruleNameModel = new RuleNameModel
            {
                Name = "Name15",
                Description = "Description15",
                Json = "Json15",
                Template = "Template15",
                SqlStr = "SqlStr15",
                SqlPart = "SqlPart15"
            };

            // ARRANGE
            ServiceResult<RuleNameDto> serviceResult = await _ruleNameService.UpdateRuleName(ruleNameModel, 1);
            RuleName? ruleName = await _dbContext.RuleNames
                .AsNoTracking()
                .FirstAsync(i => i.Id == 1);

            // ASSERT
            Assert.Equal(1, ruleName?.Id);
            Assert.Equal("Name15", ruleName?.Name);
            Assert.Equal("Description15", ruleName?.Description);
            Assert.Equal("Json15", ruleName?.Json);
            Assert.Equal("Template15", ruleName?.Template);
            Assert.Equal("SqlStr15", ruleName?.SqlStr);
            Assert.Equal("SqlPart15", ruleName?.SqlPart);
            Assert.Empty(serviceResult.Errors);
        }

        [Fact]
        public async Task Service_AddRuleName()
        {
            // ACT
            RuleNameModel ruleNameModel = new RuleNameModel
            {
                Name = "Name5",
                Description = "Description5",
                Json = "Json5",
                Template = "Template5",
                SqlStr = "SqlStr5",
                SqlPart = "SqlPart5"
            };

            // ARRANGE
            ServiceResult<RuleNameDto> serviceResult = await _ruleNameService.AddRuleName(ruleNameModel);
            RuleName? ruleName = await _dbContext.RuleNames
                .AsNoTracking()
                .OrderBy(i => i.Id).LastAsync();
            int ratingCount = await _dbContext.RuleNames.CountAsync();

            // ASSERT
            Assert.Equal(5, ruleName?.Id);
            Assert.Equal(5, ratingCount);
            Assert.Equal("Name5", ruleName?.Name);
            Assert.Equal("Description5", ruleName?.Description);
            Assert.Equal("Json5", ruleName?.Json);
            Assert.Equal("Template5", ruleName?.Template);
            Assert.Equal("SqlStr5", ruleName?.SqlStr);
            Assert.Equal("SqlPart5", ruleName?.SqlPart);
            Assert.Empty(serviceResult.Errors);
        }

        [Fact]
        public async Task Service_DeleteRuleName()
        {
            // ACT
            await _ruleNameService.DeleteRuleName(2);
            IEnumerable<RuleName> ruleNames = await _dbContext.RuleNames.ToListAsync();

            // ASSERT
            Assert.Equal(3, ruleNames.Count());
            Assert.Contains(ruleNames, i => i.Id == 1);
            Assert.Contains(ruleNames, i => i.Id == 3);
            Assert.Contains(ruleNames, i => i.Id == 4);
            Assert.DoesNotContain(ruleNames, i => i.Id == 2);
        }

    }
}
