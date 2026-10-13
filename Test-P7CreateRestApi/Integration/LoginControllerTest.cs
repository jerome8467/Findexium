using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Models.UserModel;
using System.Net;
using System.Net.Http.Json;
using Test_P7CreateRestApi.Integration.Outils;
using Xunit;

namespace Test_P7CreateRestApi.Integration.LoginTest
{
    public class LoginControllerTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly LocalDbContext _dbContext;
        private readonly CustomWebApplicationFactory _factory;

        public LoginControllerTest()
        {
            _connection = new SqliteConnection("Data source=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<LocalDbContext>()
                .UseSqlite(_connection)
                .Options;
            _dbContext = new LocalDbContext(options);
            _dbContext.Database.EnsureCreated();

            _dbContext.Users.Add(CreateUser.NewValidUser(1, "AdminTest", nameof(UserRole.Admin)));
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
        public async Task Controller_Login_ValidCredentials()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();
            LoginModel loginModel = CreateModel.NewValidLoginModel("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/Login/Access", loginModel);
            Dictionary<string, string>? content = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(content);
            Assert.False(string.IsNullOrEmpty(content!["token"]));
        }

        [Fact]
        public async Task Controller_Login_WrongPassword()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();
            LoginModel loginModel = CreateModel.NewValidLoginModel("AdminTest", "WrongPassword1!");

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/Login/Access", loginModel);

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_Login_UnknownUser()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();
            LoginModel loginModel = CreateModel.NewValidLoginModel("UnknownUser", "Password1!");

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/Login/Access", loginModel);

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_Login_InvalidModel()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();
            LoginModel loginModel = CreateModel.NewValidLoginModel("", "Password1!");

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/Login/Access", loginModel);

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}