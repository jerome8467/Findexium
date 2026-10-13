using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Models.UserModel;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Test_P7CreateRestApi.Integration.Outils;
using Xunit;

namespace Test_P7CreateRestApi.Integration.UserTest
{
    public class UserControllerTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly LocalDbContext _dbContext;
        private readonly CustomWebApplicationFactory _factory;

        // Same enum converter as the API, to read the role sent as text
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        public UserControllerTest()
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
            _dbContext.Users.Add(CreateUser.NewValidUser(3, "UserTest2", nameof(UserRole.User)));
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
        public async Task Controller_GetProfile_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.GetAsync("/User/Profile");

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_GetProfile_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("UserTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/User/Profile");
            UserDto? user = await response.Content.ReadFromJsonAsync<UserDto>(_jsonOptions);

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(user);
            Assert.Equal(2, user?.Id);
            Assert.Equal("UserTest", user?.Username);
            Assert.Equal(UserRole.User, user?.Role);
        }

        [Fact]
        public async Task Controller_AddNewUser_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();
            UserSignUpModel userSignUpModel = CreateModel.NewValidUserSignUpModel("NewUser");

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/User/Creation", userSignUpModel);
            UserDto? user = await response.Content.ReadFromJsonAsync<UserDto>(_jsonOptions);
            int userCount = await _dbContext.Users.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(user);
            Assert.Equal("NewUser", user?.Username);
            Assert.Equal(UserRole.User, user?.Role);
            Assert.Equal(4, userCount);
        }

        [Fact]
        public async Task Controller_AddNewUser_InvalidModel()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();
            UserSignUpModel userSignUpModel = CreateModel.NewValidUserSignUpModel("");

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/User/Creation", userSignUpModel);
            int userCount = await _dbContext.Users.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(3, userCount);
        }

        [Fact]
        public async Task Controller_AddNewUser_UsernameUnavailable()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();
            UserSignUpModel userSignUpModel = CreateModel.NewValidUserSignUpModel("UserTest");

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/User/Creation", userSignUpModel);
            int userCount = await _dbContext.Users.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(3, userCount);
        }

        [Fact]
        public async Task Controller_ShowUpdateForm_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.GetAsync("/User/FormUpdate");

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_ShowUpdateForm_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("UserTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/User/FormUpdate");
            UserUpdateModel? userUpdateModel = await response.Content.ReadFromJsonAsync<UserUpdateModel>();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(userUpdateModel);
            Assert.Equal("UserTest", userUpdateModel?.Username);
        }

        [Fact]
        public async Task Controller_UpdateUser_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();
            UserUpdateModel userUpdateModel = CreateModel.NewValidUserUpdateModel("UserTestUpdate");

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/User/Modification", userUpdateModel);

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_UpdateUser_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("UserTest", "Password1!");
            UserUpdateModel userUpdateModel = CreateModel.NewValidUserUpdateModel("UserTestUpdate");

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/User/Modification", userUpdateModel);
            User user = await _dbContext.Users
                .AsNoTracking()
                .FirstAsync(u => u.Id == 2);

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("UserTestUpdate", user.Username);
            Assert.Equal(nameof(UserRole.User), user.Role);
        }

        [Fact]
        public async Task Controller_UpdateUser_InvalidModel()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("UserTest", "Password1!");
            UserUpdateModel userUpdateModel = CreateModel.NewValidUserUpdateModel("");

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/User/Modification", userUpdateModel);
            User user = await _dbContext.Users
                .AsNoTracking()
                .FirstAsync(u => u.Id == 2);

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("UserTest", user.Username);
        }

        [Fact]
        public async Task Controller_UpdateUser_UsernameUnavailable()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("UserTest", "Password1!");
            UserUpdateModel userUpdateModel = CreateModel.NewValidUserUpdateModel("UserTest2");

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/User/Modification", userUpdateModel);
            User user = await _dbContext.Users
                .AsNoTracking()
                .FirstAsync(u => u.Id == 2);

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("UserTest", user.Username);
        }

        [Fact]
        public async Task Controller_ChangePassword_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();
            ChangePasswordModel changePasswordModel = CreateModel.NewValidChangePasswordModel("Password1!", "NewPassword1!");

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/User/ChangePassword", changePasswordModel);

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_ChangePassword_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("UserTest", "Password1!");
            ChangePasswordModel changePasswordModel = CreateModel.NewValidChangePasswordModel("Password1!", "NewPassword1!");
            LoginModel loginModel = CreateModel.NewValidLoginModel("UserTest", "NewPassword1!");

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/User/ChangePassword", changePasswordModel);
            HttpResponseMessage loginResponse = await _factory.CreateClient().PostAsJsonAsync("/Login/Access", loginModel);

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
        }

        [Fact]
        public async Task Controller_ChangePassword_WrongOldPassword()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("UserTest", "Password1!");
            ChangePasswordModel changePasswordModel = CreateModel.NewValidChangePasswordModel("WrongPassword1!", "NewPassword1!");
            LoginModel loginModel = CreateModel.NewValidLoginModel("UserTest", "NewPassword1!");

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/User/ChangePassword", changePasswordModel);
            HttpResponseMessage loginResponse = await _factory.CreateClient().PostAsJsonAsync("/Login/Access", loginModel);

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, loginResponse.StatusCode);
        }

        [Fact]
        public async Task Controller_ChangePassword_InvalidModel()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("UserTest", "Password1!");
            ChangePasswordModel changePasswordModel = CreateModel.NewValidChangePasswordModel("Password1!", "abc");

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/User/ChangePassword", changePasswordModel);

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}