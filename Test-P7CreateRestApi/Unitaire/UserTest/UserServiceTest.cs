using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Models.UserModel;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Service;
using P7CreateRestApi.Service.Interfaces;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Test_P7CreateRestApi.Unitaire.UserTest
{
    public class UserServiceTest : IDisposable
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly LocalDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserServiceTest()
        {
            var options = new DbContextOptionsBuilder<LocalDbContext>()
                .UseSqlite("Data source=:memory:")
                .Options;
            _dbContext = new LocalDbContext(options);
            _dbContext.Database.OpenConnection();
            _dbContext.Database.EnsureCreated();

            _passwordHasher = new PasswordHasher<User>();

            _dbContext.Users.Add(CreateValidUser(1, "Username1", "Password1!", "Admin"));
            _dbContext.Users.Add(CreateValidUser(2, "Username2", "Password2!", "User"));
            _dbContext.Users.Add(CreateValidUser(3, "Username3", "Password3!", "User"));
            _dbContext.Users.Add(CreateValidUser(4, "Username4", "Password4!", "User"));
            _dbContext.SaveChanges();

            _userRepository = new UserRepository(_dbContext);

            var inMemorySettings = new Dictionary<string, string?>
            {
                { "Jwt:Key", "CleDeTestPourUnitTestTresLonguePourP7" },
                { "Jwt:Issuer", "P7CreateRestApiTest" },
                { "Jwt:Audience", "P7CreateRestApiTest" },
                { "Jwt:ExpireMinutes", "60" }
            };
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            TokenService tokenService = new TokenService(configuration);

            _userService = new UserService(_userRepository, _passwordHasher, tokenService);
        }

        public User CreateValidUser(int id, string username, string password, string role)
        {
            User user = new User
            {
                Id = id,
                Username = username,
                Fullname = "Test",
                Role = role
            };
            user.Password = _passwordHasher.HashPassword(user, password);

            return user;
        }

        public void Dispose()
        {
            _dbContext.Database.CloseConnection();
            _dbContext.Dispose();
        }

        [Fact]
        public async Task Service_GetAllUser()
        {
            // ACT
            IEnumerable<UserDto> users = await _userService.GetAllUser();

            // ASSERT
            Assert.Equal(4, users.Count());
            Assert.Contains(users, u => u.Username == "Username1" && u.Role == UserRole.Admin);
            Assert.Contains(users, u => u.Username == "Username3" && u.Role == UserRole.User);
        }

        [Fact]
        public async Task Service_GetUserByUsername()
        {
            // ACT
            UserDto? userDto = await _userService.GetUserByUsername("Username2");

            // ASSERT
            Assert.NotNull(userDto);
            Assert.Equal(2, userDto?.Id);
            Assert.Equal("Username2", userDto?.Username);
            Assert.Equal(UserRole.User, userDto?.Role);
        }

        [Fact]
        public async Task Service_GetUserById()
        {
            // ACT
            UserDto? userDto = await _userService.GetUserById(1);

            // ASSERT
            Assert.NotNull(userDto);
            Assert.Equal(1, userDto?.Id);
            Assert.Equal("Username1", userDto?.Username);
            Assert.Equal(UserRole.Admin, userDto?.Role);
        }

        [Fact]
        public async Task Service_GetUpdateUserModelById()
        {
            // ACT
            AdminUpdateModel? adminUpdateModel = await _userService.GetAdminUpdateModelById(3);

            // ASSERT
            Assert.NotNull(adminUpdateModel);
            Assert.Equal("Username3", adminUpdateModel?.Username);
            Assert.Equal(UserRole.User, adminUpdateModel?.Role);
        }

        [Fact]
        public async Task Service_AddUserByAdmin()
        {
            // ARRANGE
            AdminSignUpModel adminSignUpModel = new AdminSignUpModel
            {
                Username = "Username5",
                Password = "Password5!",
                Fullname = "Test",
                Role = UserRole.User
            };

            // ACT
            ServiceResult<UserDto> serviceResult = await _userService.AddUserByAdmin(adminSignUpModel);
            User? user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == "Username5");
            int userCount = await _dbContext.Users.CountAsync();

            // ASSERT
            Assert.Empty(serviceResult.Errors);
            Assert.NotNull(user);
            Assert.Equal(5, user?.Id);
            Assert.Equal("Username5", user?.Username);
            Assert.Equal("User", user?.Role);
            Assert.NotEqual("Password5!", user?.Password);
            Assert.Equal(5, userCount);
        }

        [Fact]
        public async Task Service_UpdateUserByAdmin()
        {
            // ARRANGE
            AdminUpdateModel adminUpdateModel = new AdminUpdateModel
            {
                Username = "Username1Update",
                Fullname = "FullnameUpdate",
                Role = UserRole.User
            };

            // ACT
            ServiceResult<UserDto> serviceResult = await _userService.UpdateUserByAdmin(adminUpdateModel, 1);
            User user = await _dbContext.Users
                .AsNoTracking()
                .FirstAsync(u => u.Id == 1);

            // ASSERT
            Assert.Empty(serviceResult.Errors);
            Assert.Equal("Username1Update", user.Username);
            Assert.Equal("FullnameUpdate", user.Fullname);
            Assert.Equal("User", user.Role);
        }

        [Fact]
        public async Task Service_AddUser()
        {
            // ARRANGE
            UserSignUpModel userSignUpModel = new UserSignUpModel
            {
                Username = "Username5",
                Password = "Password5!",
                Fullname = "Test"
            };

            // ACT
            ServiceResult<UserDto> serviceResult = await _userService.AddUser(userSignUpModel);
            User? user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == "Username5");
            int userCount = await _dbContext.Users.CountAsync();

            // ASSERT
            Assert.Empty(serviceResult.Errors);
            Assert.NotNull(user);
            Assert.Equal(5, user?.Id);
            Assert.Equal("Username5", user?.Username);
            Assert.Equal("User", user?.Role);
            Assert.NotEqual("Password5!", user?.Password);
            Assert.Equal(5, userCount);
        }

        [Fact]
        public async Task Service_GetUserUpdateModelById()
        {
            // ACT
            UserUpdateModel? userUpdateModel = await _userService.GetUserUpdateModelById(3);

            // ASSERT
            Assert.NotNull(userUpdateModel);
            Assert.Equal("Username3", userUpdateModel?.Username);
            Assert.Equal("Test", userUpdateModel?.Fullname);
        }

        [Fact]
        public async Task Service_UpdateUser()
        {
            // ARRANGE
            UserUpdateModel userUpdateModel = new UserUpdateModel
            {
                Username = "Username2Update",
                Fullname = "FullnameUpdate"
            };

            // ACT
            ServiceResult<UserDto> serviceResult = await _userService.UpdateUser(userUpdateModel, 2);
            User user = await _dbContext.Users
                .AsNoTracking()
                .FirstAsync(u => u.Id == 2);

            // ASSERT
            Assert.Empty(serviceResult.Errors);
            Assert.Equal("Username2Update", user.Username);
            Assert.Equal("FullnameUpdate", user.Fullname);
        }

        [Fact]
        public async Task Service_ChangePassword()
        {
            // ARRANGE
            ChangePasswordModel changePasswordModel = new ChangePasswordModel
            {
                OldPassword = "Password1!",
                NewPassword = "NouveauMotDePasse1!"
            };

            // ACT
            ServiceResult<UserDto> serviceResult = await _userService.ChangePassword(changePasswordModel, 1);
            User user = await _dbContext.Users
                .AsNoTracking()
                .FirstAsync(u => u.Id == 1);
            PasswordVerificationResult oldPasswordResult = _passwordHasher
                .VerifyHashedPassword(user, user.Password, "Password1!");
            PasswordVerificationResult newPasswordResult = _passwordHasher
                .VerifyHashedPassword(user, user.Password, "NouveauMotDePasse1!");

            // ASSERT
            Assert.Empty(serviceResult.Errors);
            Assert.NotNull(user);
            Assert.Equal("Username1", user.Username);
            Assert.Equal(PasswordVerificationResult.Failed, oldPasswordResult);
            Assert.Equal(PasswordVerificationResult.Success, newPasswordResult);
        }

        [Fact]
        public async Task Service_DeleteUserByAdmin()
        {
            // ACT
            List<ValidationResult> errors = await _userService.DeleteUserByAdmin(2);
            IEnumerable<User> users = await _dbContext.Users
                .AsNoTracking()
                .ToListAsync();

            // ASSERT
            Assert.Empty(errors);
            Assert.Equal(3, users.Count());
            Assert.Contains(users, u => u.Id == 1);
            Assert.Contains(users, u => u.Id == 3);
            Assert.Contains(users, u => u.Id == 4);
            Assert.DoesNotContain(users, u => u.Id == 2);
        }

        [Fact]
        public async Task Service_Login()
        {
            // ARRANGE
            LoginModel loginModel = new LoginModel
            {
                Username = "Username1",
                Password = "Password1!"
            };

            // ACT
            string? token = await _userService.Login(loginModel);

            // ASSERT
            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }
    }
}