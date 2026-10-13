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

namespace Test_P7CreateRestApi.Integration.AdminTest
{
    public class AdminControllerTest : IDisposable
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

        public AdminControllerTest()
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

        /////////////////////////// LIST ///////////////////////////

        [Fact]
        public async Task Controller_GetAllUsers_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Admin/User/List");

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_GetAllUsers_WithUserToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("UserTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Admin/User/List");

            // ASSERT
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Controller_GetAllUsers_WithAdminToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Admin/User/List");
            List<UserDto>? users = await response.Content.ReadFromJsonAsync<List<UserDto>>(_jsonOptions);

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(users);
            Assert.Equal(3, users?.Count);
            Assert.Contains(users, u => u.Id == 1 && u.Username == "AdminTest");
        }

        /////////////////////////// DETAILS BY USERNAME ///////////////////////////

        [Fact]
        public async Task Controller_GetUserByUsername_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Admin/User/DetailsByUsername/UserTest");

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_GetUserByUsername_WithUserToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("UserTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Admin/User/DetailsByUsername/UserTest");

            // ASSERT
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Controller_GetUserByUsername_WithAdminToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Admin/User/DetailsByUsername/UserTest");
            UserDto? user = await response.Content.ReadFromJsonAsync<UserDto>(_jsonOptions);

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(user);
            Assert.Equal(2, user?.Id);
            Assert.Equal("UserTest", user?.Username);
        }

        [Fact]
        public async Task Controller_GetUserByUsername_NotFound()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Admin/User/DetailsByUsername/UnknownUser");

            // ASSERT
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        /////////////////////////// DETAILS BY ID ///////////////////////////

        [Fact]
        public async Task Controller_GetUserById_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Admin/User/DetailsById/2");

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_GetUserById_WithUserToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("UserTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Admin/User/DetailsById/2");

            // ASSERT
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Controller_GetUserById_WithAdminToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Admin/User/DetailsById/2");
            UserDto? user = await response.Content.ReadFromJsonAsync<UserDto>(_jsonOptions);

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(user);
            Assert.Equal(2, user?.Id);
            Assert.Equal(UserRole.User, user?.Role);
        }

        [Fact]
        public async Task Controller_GetUserById_NotFound()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Admin/User/DetailsById/999");

            // ASSERT
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        /////////////////////////// CREATION ///////////////////////////

        [Fact]
        public async Task Controller_AddNewUser_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();
            AdminSignUpModel adminSignUpModel = CreateModel.NewValidAdminSignUpModel("NewAdmin", UserRole.Admin);

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/Admin/User/Creation", adminSignUpModel);
            int userCount = await _dbContext.Users.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal(3, userCount);
        }

        [Fact]
        public async Task Controller_AddNewUser_WithUserToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("UserTest", "Password1!");
            AdminSignUpModel adminSignUpModel = CreateModel.NewValidAdminSignUpModel("NewAdmin", UserRole.Admin);

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/Admin/User/Creation", adminSignUpModel);
            int userCount = await _dbContext.Users.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Equal(3, userCount);
        }

        [Fact]
        public async Task Controller_AddNewUser_WithAdminToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            AdminSignUpModel adminSignUpModel = CreateModel.NewValidAdminSignUpModel("NewAdmin", UserRole.Admin);

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/Admin/User/Creation", adminSignUpModel);
            UserDto? user = await response.Content.ReadFromJsonAsync<UserDto>(_jsonOptions);
            int userCount = await _dbContext.Users.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(user);
            Assert.Equal("NewAdmin", user?.Username);
            Assert.Equal(UserRole.Admin, user?.Role);
            Assert.Equal(4, userCount);
        }

        [Fact]
        public async Task Controller_AddNewUser_InvalidModel()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            AdminSignUpModel adminSignUpModel = CreateModel.NewValidAdminSignUpModel("", UserRole.Admin);

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/Admin/User/Creation", adminSignUpModel);
            int userCount = await _dbContext.Users.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(3, userCount);
        }

        [Fact]
        public async Task Controller_AddNewUser_InvalidRole()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            AdminSignUpModel adminSignUpModel = CreateModel.NewValidAdminSignUpModel("NewAdmin", (UserRole)99);

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/Admin/User/Creation", adminSignUpModel);
            int userCount = await _dbContext.Users.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(3, userCount);
        }

        [Fact]
        public async Task Controller_AddNewUser_UsernameUnavailable()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            AdminSignUpModel adminSignUpModel = CreateModel.NewValidAdminSignUpModel("UserTest", UserRole.User);

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/Admin/User/Creation", adminSignUpModel);
            int userCount = await _dbContext.Users.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(3, userCount);
        }

        /////////////////////////// FORM UPDATE ///////////////////////////

        [Fact]
        public async Task Controller_ShowUpdateForm_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Admin/User/FormUpdate/2");

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_ShowUpdateForm_WithUserToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("UserTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Admin/User/FormUpdate/2");

            // ASSERT
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Controller_ShowUpdateForm_WithAdminToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Admin/User/FormUpdate/2");
            AdminUpdateModel? adminUpdateModel = await response.Content.ReadFromJsonAsync<AdminUpdateModel>(_jsonOptions);

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(adminUpdateModel);
            Assert.Equal("UserTest", adminUpdateModel?.Username);
            Assert.Equal(UserRole.User, adminUpdateModel?.Role);
        }

        [Fact]
        public async Task Controller_ShowUpdateForm_NotFound()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Admin/User/FormUpdate/999");

            // ASSERT
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        /////////////////////////// MODIFICATION ///////////////////////////

        [Fact]
        public async Task Controller_UpdateUser_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();
            AdminUpdateModel adminUpdateModel = CreateModel.NewValidAdminUpdateModel("UserTestUpdate", UserRole.Admin);

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/Admin/User/Modification/2", adminUpdateModel);

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_UpdateUser_WithUserToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("UserTest", "Password1!");
            AdminUpdateModel adminUpdateModel = CreateModel.NewValidAdminUpdateModel("UserTestUpdate", UserRole.Admin);

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/Admin/User/Modification/2", adminUpdateModel);
            User user = await _dbContext.Users
                .AsNoTracking()
                .FirstAsync(u => u.Id == 2);

            // ASSERT
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Equal("UserTest", user.Username);
            Assert.Equal(nameof(UserRole.User), user.Role);
        }

        [Fact]
        public async Task Controller_UpdateUser_WithAdminToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            AdminUpdateModel adminUpdateModel = CreateModel.NewValidAdminUpdateModel("UserTestUpdate", UserRole.Admin);

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/Admin/User/Modification/2", adminUpdateModel);
            User user = await _dbContext.Users
                .AsNoTracking()
                .FirstAsync(u => u.Id == 2);

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("UserTestUpdate", user.Username);
            Assert.Equal(nameof(UserRole.Admin), user.Role);
        }

        [Fact]
        public async Task Controller_UpdateUser_InvalidModel()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            AdminUpdateModel adminUpdateModel = CreateModel.NewValidAdminUpdateModel("", UserRole.Admin);

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/Admin/User/Modification/2", adminUpdateModel);
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
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            AdminUpdateModel adminUpdateModel = CreateModel.NewValidAdminUpdateModel("UserTest2", UserRole.User);

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/Admin/User/Modification/2", adminUpdateModel);
            User user = await _dbContext.Users
                .AsNoTracking()
                .FirstAsync(u => u.Id == 2);

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("UserTest", user.Username);
        }

        [Fact]
        public async Task Controller_UpdateUser_NotFound()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            AdminUpdateModel adminUpdateModel = CreateModel.NewValidAdminUpdateModel("UserTestUpdate", UserRole.User);

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/Admin/User/Modification/999", adminUpdateModel);

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        /////////////////////////// REMOVAL ///////////////////////////

        [Fact]
        public async Task Controller_DeleteUser_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.DeleteAsync("/Admin/User/Removal/2");
            int userCount = await _dbContext.Users.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal(3, userCount);
        }

        [Fact]
        public async Task Controller_DeleteUser_WithUserToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("UserTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.DeleteAsync("/Admin/User/Removal/3");
            int userCount = await _dbContext.Users.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Equal(3, userCount);
        }

        [Fact]
        public async Task Controller_DeleteUser_WithAdminToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.DeleteAsync("/Admin/User/Removal/3");
            IEnumerable<User> users = await _dbContext.Users
                .AsNoTracking()
                .ToListAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, users.Count());
            Assert.DoesNotContain(users, u => u.Id == 3);
        }

        [Fact]
        public async Task Controller_DeleteUser_NotFound()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.DeleteAsync("/Admin/User/Removal/999");
            int userCount = await _dbContext.Users.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(3, userCount);
        }
    }
}