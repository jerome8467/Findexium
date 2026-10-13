using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;
using P7CreateRestApi.Models.UserModel;
using System.Net;
using System.Net.Http.Json;
using Test_P7CreateRestApi.Integration.Outils;
using Xunit;

namespace Test_P7CreateRestApi.Integration.RuleNameTest
{
    public class RuleNameControllerTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly LocalDbContext _dbContext;
        private readonly CustomWebApplicationFactory _factory;

        public RuleNameControllerTest()
        {
            _connection = new SqliteConnection("Data source=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<LocalDbContext>()
                .UseSqlite(_connection)
                .Options;
            _dbContext = new LocalDbContext(options);
            _dbContext.Database.EnsureCreated();

            _dbContext.Users.Add(CreateUser.NewValidUser(1, "AdminTest", nameof(UserRole.Admin)));

            _dbContext.RuleNames.Add(CreateEntity.NewValidRuleName(1, "Name1", "Description1", "Json1", "Template1", "SqlStr1", "SqlPart1"));
            _dbContext.RuleNames.Add(CreateEntity.NewValidRuleName(2, "Name2", "Description2", "Json2", "Template2", "SqlStr2", "SqlPart2"));
            _dbContext.RuleNames.Add(CreateEntity.NewValidRuleName(3, "Name3", "Description3", "Json3", "Template3", "SqlStr3", "SqlPart3"));
            _dbContext.RuleNames.Add(CreateEntity.NewValidRuleName(4, "Name4", "Description4", "Json4", "Template4", "SqlStr4", "SqlPart4"));
            _dbContext.SaveChanges();

            _factory = new CustomWebApplicationFactory(_connection);
        }

        public void Dispose()
        {
            _factory.Dispose();
            _dbContext.Dispose();
            _connection.Dispose();
        }

        [Fact]
        public async Task Controller_GetAllRuleName_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.GetAsync("/RuleName/List");

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_GetAllRuleName_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/RuleName/List");
            List<RuleNameDto>? ruleNames = await response.Content.ReadFromJsonAsync<List<RuleNameDto>>();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(ruleNames);
            Assert.Equal(4, ruleNames?.Count);
            Assert.Contains(ruleNames, r => r.Id == 1 && r.Name == "Name1");
        }

        [Fact]
        public async Task Controller_GetRuleNameById_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.GetAsync("/RuleName/Details/1");

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_GetRuleNameById_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/RuleName/Details/1");
            RuleNameDto? ruleName = await response.Content.ReadFromJsonAsync<RuleNameDto>();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(ruleName);
            Assert.Equal(1, ruleName?.Id);
            Assert.Equal("Name1", ruleName?.Name);
        }

        [Fact]
        public async Task Controller_GetRuleNameById_NotFound()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/RuleName/Details/999");

            // ASSERT
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Controller_AddNewRuleName_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();
            RuleNameModel ruleNameModel = CreateModel.NewValidRuleNameModel("Name5");

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/RuleName/Creation", ruleNameModel);

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_AddNewRuleName_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            RuleNameModel ruleNameModel = CreateModel.NewValidRuleNameModel("Name5");

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/RuleName/Creation", ruleNameModel);
            RuleNameDto? ruleName = await response.Content.ReadFromJsonAsync<RuleNameDto>();
            int ruleNameCount = await _dbContext.RuleNames.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(ruleName);
            Assert.Equal("Name5", ruleName?.Name);
            Assert.Equal(5, ruleNameCount);
        }

        [Fact]
        public async Task Controller_AddNewRuleName_InvalidModel()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            RuleNameModel ruleNameModel = CreateModel.NewValidRuleNameModel("");

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/RuleName/Creation", ruleNameModel);
            int ruleNameCount = await _dbContext.RuleNames.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(4, ruleNameCount);
        }

        [Fact]
        public async Task Controller_ShowUpdateForm_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.GetAsync("/RuleName/FormUpdate/1");

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_ShowUpdateForm_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/RuleName/FormUpdate/1");
            RuleNameModel? ruleNameModel = await response.Content.ReadFromJsonAsync<RuleNameModel>();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(ruleNameModel);
            Assert.Equal("Name1", ruleNameModel?.Name);
        }

        [Fact]
        public async Task Controller_ShowUpdateForm_NotFound()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/RuleName/FormUpdate/999");

            // ASSERT
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Controller_UpdateRuleName_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();
            RuleNameModel ruleNameModel = CreateModel.NewValidRuleNameModel("Name1Update");

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/RuleName/Modification/1", ruleNameModel);

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_UpdateRuleName_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            RuleNameModel ruleNameModel = CreateModel.NewValidRuleNameModel("Name1Update");

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/RuleName/Modification/1", ruleNameModel);
            RuleName ruleName = await _dbContext.RuleNames
                .AsNoTracking()
                .FirstAsync(r => r.Id == 1);

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Name1Update", ruleName.Name);
        }

        [Fact]
        public async Task Controller_UpdateRuleName_InvalidModel()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            RuleNameModel ruleNameModel = CreateModel.NewValidRuleNameModel("");

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/RuleName/Modification/1", ruleNameModel);
            RuleName ruleName = await _dbContext.RuleNames
                .AsNoTracking()
                .FirstAsync(r => r.Id == 1);

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Name1", ruleName.Name);
        }

        [Fact]
        public async Task Controller_UpdateRuleName_NotFound()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            RuleNameModel ruleNameModel = CreateModel.NewValidRuleNameModel("Name1Update");

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/RuleName/Modification/999", ruleNameModel);

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Controller_DeleteRuleName_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.DeleteAsync("/RuleName/Removal/1");
            int ruleNameCount = await _dbContext.RuleNames.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal(4, ruleNameCount);
        }

        [Fact]
        public async Task Controller_DeleteRuleName_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.DeleteAsync("/RuleName/Removal/1");
            IEnumerable<RuleName> ruleNames = await _dbContext.RuleNames
                .AsNoTracking()
                .ToListAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(3, ruleNames.Count());
            Assert.DoesNotContain(ruleNames, r => r.Id == 1);
        }

        [Fact]
        public async Task Controller_DeleteRuleName_NotFound()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.DeleteAsync("/RuleName/Removal/999");
            int ruleNameCount = await _dbContext.RuleNames.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(4, ruleNameCount);
        }
    }
}