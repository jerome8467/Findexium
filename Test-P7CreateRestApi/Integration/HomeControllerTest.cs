using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Models.UserModel;
using System.Net;
using Test_P7CreateRestApi.Integration.Outils;
using Xunit;

namespace Test_P7CreateRestApi.Integration.HomeTest
{
    public class HomeControllerTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly LocalDbContext _dbContext;
        private readonly CustomWebApplicationFactory _factory;

        public HomeControllerTest()
        {
            _connection = new SqliteConnection("Data source=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<LocalDbContext>()
                .UseSqlite(_connection)
                .Options;
            _dbContext = new LocalDbContext(options);
            _dbContext.Database.EnsureCreated();

            _dbContext.Users.Add(CreateUser.NewValidUser(1, "AdminTest", nameof(UserRole.Admin)));
            _dbContext.Users.Add(CreateUser.NewValidUser(2, "UserTest", nameof(UserRole.User)));
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
        public async Task Controller_Home_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Home");

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Controller_HomeAdmin_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Home/Admin");

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_HomeAdmin_WithUserToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("UserTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Home/Admin");

            // ASSERT
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Controller_HomeAdmin_WithAdminToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Home/Admin");

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}