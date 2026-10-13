using Microsoft.AspNetCore.Identity;
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

namespace Test_P7CreateRestApi.Integration.BidListTest
{
    public class BidListControllerTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly LocalDbContext _dbContext;
        private readonly CustomWebApplicationFactory _factory;

        public BidListControllerTest()
        {
            _connection = new SqliteConnection("Data source=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<LocalDbContext>()
                .UseSqlite(_connection)
                .Options;
            _dbContext = new LocalDbContext(options);
            _dbContext.Database.EnsureCreated();

            _dbContext.Users.Add(CreateUser.NewValidUser(1, "AdminTest", nameof(UserRole.Admin)));

            _dbContext.BidLists.Add(CreateEntity.NewValidBidList(1, "Account1", "UserNameTempo1"));
            _dbContext.BidLists.Add(CreateEntity.NewValidBidList(2, "Account2", "UserNameTempo2"));
            _dbContext.BidLists.Add(CreateEntity.NewValidBidList(3, "Account3", "UserNameTempo3"));
            _dbContext.BidLists.Add(CreateEntity.NewValidBidList(4, "Account4", "UserNameTempo4"));
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
        public async Task Controller_GetAllBidList_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.GetAsync("/BidList/List");

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_GetAllBidList_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/BidList/List");
            List<BidListDto>? bidLists = await response.Content.ReadFromJsonAsync<List<BidListDto>>();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(bidLists);
            Assert.Equal(4, bidLists?.Count);
            Assert.Contains(bidLists, b => b.BidListId == 1 && b.Account == "Account1");
        }

        [Fact]
        public async Task Controller_GetBidListById_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.GetAsync("/BidList/Details/1");

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_GetBidListById_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/BidList/Details/1");
            BidListDto? bidList = await response.Content.ReadFromJsonAsync<BidListDto>();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(bidList);
            Assert.Equal(1, bidList?.BidListId);
            Assert.Equal("Account1", bidList?.Account);
        }

        [Fact]
        public async Task Controller_GetBidListById_NotFound()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/BidList/Details/999");

            // ASSERT
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Controller_AddNewBidList_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();
            BidListModel bidListModel = CreateModel.NewValidBidListModel("Account5");

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/BidList/Creation", bidListModel);

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_AddNewBidList_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            BidListModel bidListModel = CreateModel.NewValidBidListModel("Account5");

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/BidList/Creation", bidListModel);
            BidListDto? bidList = await response.Content.ReadFromJsonAsync<BidListDto>();
            int bidListCount = await _dbContext.BidLists.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(bidList);
            Assert.Equal("Account5", bidList?.Account);
            Assert.Equal("AdminTest", bidList?.CreationName);
            Assert.Equal(5, bidListCount);
        }

        [Fact]
        public async Task Controller_AddNewBidList_InvalidModel()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            BidListModel bidListModel = CreateModel.NewValidBidListModel("");

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/BidList/Creation", bidListModel);
            int bidListCount = await _dbContext.BidLists.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(4, bidListCount);
        }

        [Fact]
        public async Task Controller_ShowUpdateForm_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.GetAsync("/BidList/FormUpdate/1");

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_ShowUpdateForm_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/BidList/FormUpdate/1");
            BidListModel? bidListModel = await response.Content.ReadFromJsonAsync<BidListModel>();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(bidListModel);
            Assert.Equal("Account1", bidListModel?.Account);
        }

        [Fact]
        public async Task Controller_ShowUpdateForm_NotFound()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/BidList/FormUpdate/999");

            // ASSERT
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Controller_UpdateBidList_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();
            BidListModel bidListModel = CreateModel.NewValidBidListModel("Account1Update");

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/BidList/Modification/1", bidListModel);

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_UpdateBidList_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            BidListModel bidListModel = CreateModel.NewValidBidListModel("Account1Update");

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/BidList/Modification/1", bidListModel);
            BidList bidList = await _dbContext.BidLists
                .AsNoTracking()
                .FirstAsync(b => b.BidListId == 1);

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Account1Update", bidList.Account);
            Assert.Equal("AdminTest", bidList.RevisionName);
            Assert.Equal("UserNameTempo1", bidList.CreationName);
        }

        [Fact]
        public async Task Controller_UpdateBidList_InvalidModel()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            BidListModel bidListModel = CreateModel.NewValidBidListModel("");

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/BidList/Modification/1", bidListModel);
            BidList bidList = await _dbContext.BidLists
                .AsNoTracking()
                .FirstAsync(b => b.BidListId == 1);

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Account1", bidList.Account);
        }

        [Fact]
        public async Task Controller_UpdateBidList_NotFound()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            BidListModel bidListModel = CreateModel.NewValidBidListModel("Account1Update");

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/BidList/Modification/999", bidListModel);

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Controller_DeleteBidList_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.DeleteAsync("/BidList/Removal/1");
            int bidListCount = await _dbContext.BidLists.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal(4, bidListCount);
        }

        [Fact]
        public async Task Controller_DeleteBidList_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.DeleteAsync("/BidList/Removal/1");
            IEnumerable<BidList> bidLists = await _dbContext.BidLists
                .AsNoTracking()
                .ToListAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(3, bidLists.Count());
            Assert.DoesNotContain(bidLists, b => b.BidListId == 1);
        }

        [Fact]
        public async Task Controller_DeleteBidList_NotFound()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.DeleteAsync("/BidList/Removal/999");
            int bidListCount = await _dbContext.BidLists.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(4, bidListCount);
        }
    }
}