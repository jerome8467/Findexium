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

namespace Test_P7CreateRestApi.Integration.TradeTest
{
    public class TradeControllerTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly LocalDbContext _dbContext;
        private readonly CustomWebApplicationFactory _factory;

        public TradeControllerTest()
        {
            _connection = new SqliteConnection("Data source=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<LocalDbContext>()
                .UseSqlite(_connection)
                .Options;
            _dbContext = new LocalDbContext(options);
            _dbContext.Database.EnsureCreated();

            _dbContext.Users.Add(CreateUser.NewValidUser(1, "AdminTest", nameof(UserRole.Admin)));

            _dbContext.Trades.Add(CreateEntity.NewValidTrade(1, "Account1", "UserNameTempo1"));
            _dbContext.Trades.Add(CreateEntity.NewValidTrade(2, "Account2", "UserNameTempo2"));
            _dbContext.Trades.Add(CreateEntity.NewValidTrade(3, "Account3", "UserNameTempo3"));
            _dbContext.Trades.Add(CreateEntity.NewValidTrade(4, "Account4", "UserNameTempo4"));
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
        public async Task Controller_GetAllTrade_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Trade/List");

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_GetAllTrade_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Trade/List");
            List<TradeDto>? trades = await response.Content.ReadFromJsonAsync<List<TradeDto>>();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(trades);
            Assert.Equal(4, trades?.Count);
            Assert.Contains(trades, t => t.TradeId == 1 && t.Account == "Account1");
        }

        [Fact]
        public async Task Controller_GetTradeById_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Trade/Details/1");

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_GetTradeById_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Trade/Details/1");
            TradeDto? trade = await response.Content.ReadFromJsonAsync<TradeDto>();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(trade);
            Assert.Equal(1, trade?.TradeId);
            Assert.Equal("Account1", trade?.Account);
        }

        [Fact]
        public async Task Controller_GetTradeById_NotFound()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Trade/Details/999");

            // ASSERT
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Controller_AddNewTrade_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();
            TradeModel tradeModel = CreateModel.NewValidTradeModel("Account5");

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/Trade/Creation", tradeModel);

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_AddNewTrade_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            TradeModel tradeModel = CreateModel.NewValidTradeModel("Account5");

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/Trade/Creation", tradeModel);
            TradeDto? trade = await response.Content.ReadFromJsonAsync<TradeDto>();
            int tradeCount = await _dbContext.Trades.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(trade);
            Assert.Equal("Account5", trade?.Account);
            Assert.Equal("AdminTest", trade?.CreationName);
            Assert.Equal(5, tradeCount);
        }

        [Fact]
        public async Task Controller_AddNewTrade_InvalidModel()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            TradeModel tradeModel = CreateModel.NewValidTradeModel("");

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/Trade/Creation", tradeModel);
            int tradeCount = await _dbContext.Trades.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(4, tradeCount);
        }

        [Fact]
        public async Task Controller_ShowUpdateForm_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Trade/FormUpdate/1");

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_ShowUpdateForm_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Trade/FormUpdate/1");
            TradeModel? tradeModel = await response.Content.ReadFromJsonAsync<TradeModel>();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(tradeModel);
            Assert.Equal("Account1", tradeModel?.Account);
        }

        [Fact]
        public async Task Controller_ShowUpdateForm_NotFound()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Trade/FormUpdate/999");

            // ASSERT
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Controller_UpdateTrade_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();
            TradeModel tradeModel = CreateModel.NewValidTradeModel("Account1Update");

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/Trade/Modification/1", tradeModel);

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_UpdateTrade_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            TradeModel tradeModel = CreateModel.NewValidTradeModel("Account1Update");

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/Trade/Modification/1", tradeModel);
            Trade trade = await _dbContext.Trades
                .AsNoTracking()
                .FirstAsync(t => t.TradeId == 1);

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Account1Update", trade.Account);
            Assert.Equal("AdminTest", trade.RevisionName);
            Assert.Equal("UserNameTempo1", trade.CreationName);
        }

        [Fact]
        public async Task Controller_UpdateTrade_InvalidModel()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            TradeModel tradeModel = CreateModel.NewValidTradeModel("");

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/Trade/Modification/1", tradeModel);
            Trade trade = await _dbContext.Trades
                .AsNoTracking()
                .FirstAsync(t => t.TradeId == 1);

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Account1", trade.Account);
        }

        [Fact]
        public async Task Controller_UpdateTrade_NotFound()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            TradeModel tradeModel = CreateModel.NewValidTradeModel("Account1Update");

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/Trade/Modification/999", tradeModel);

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Controller_DeleteTrade_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.DeleteAsync("/Trade/Removal/1");
            int tradeCount = await _dbContext.Trades.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal(4, tradeCount);
        }

        [Fact]
        public async Task Controller_DeleteTrade_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.DeleteAsync("/Trade/Removal/1");
            IEnumerable<Trade> trades = await _dbContext.Trades
                .AsNoTracking()
                .ToListAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(3, trades.Count());
            Assert.DoesNotContain(trades, t => t.TradeId == 1);
        }

        [Fact]
        public async Task Controller_DeleteTrade_NotFound()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.DeleteAsync("/Trade/Removal/999");
            int tradeCount = await _dbContext.Trades.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(4, tradeCount);
        }
    }
}