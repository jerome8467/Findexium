using Microsoft.AspNetCore.Identity;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Models.UserModel;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Ressource;
using P7CreateRestApi.Service.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace P7CreateRestApi.Service
{
    public class UserService: IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly TokenService _tokenService;


        public UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, TokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Returns all users in DTO format.
        /// </summary>
        public async Task<IEnumerable<UserDto>> GetAllUser()
        {
            IEnumerable<User> UsersList = await _userRepository.GetAllUser();
            List<UserDto> UsersDtoList = new List<UserDto>();
            foreach (User user in UsersList)
            {
                UsersDtoList.Add(MappingUserToDto(user));
            }
            return UsersDtoList;
        }

        /// <summary>
        /// Returns a user by username in DTO format, or null if it does not exist.
        /// </summary>
        public async Task<UserDto?> GetUserByUsername(string username)
        {
            User? user = await _userRepository.GetUserByUsername(username);
            if (user == null)
                return null;

            return MappingUserToDto(user);
        }

        /// <summary>
        /// Returns a user by id in DTO format, or null if it does not exist.
        /// </summary>
        public async Task<UserDto?> GetUserById(int id)
        {
            User? user = await _userRepository.GetUserById(id);
            if (user == null)
                return null;

            return MappingUserToDto(user);
        }

        /// <summary>
        /// Creates a user with the role chosen by the admin and returns it in DTO format, or the validation errors.
        /// </summary>
        public async Task<ServiceResult<UserDto>> AddUserByAdmin(AdminSignUpModel adminSignUpModel)
        {
            ValidationContext context = new ValidationContext(adminSignUpModel);
            var result = new ServiceResult<UserDto>();

            if (!Validator.TryValidateObject(adminSignUpModel, context, result.Errors, true))
                return result;

            User? findUser = await _userRepository.GetUserByUsername(adminSignUpModel.Username);
            if (findUser != null)
            {
                result.Errors.Add(new ValidationResult(UserModelRessources.UsernameUnavailable));
                return result;
            }

            User user = new User
            {
                Username = adminSignUpModel.Username,
                Fullname = adminSignUpModel.Fullname,
                Role = adminSignUpModel.Role.ToString()
            };
            user.Password = _passwordHasher.HashPassword(user, adminSignUpModel.Password);

            await _userRepository.AddUser(user);

            result.Data = MappingUserToDto(user);
            return result;
        }

        /// <summary>
        /// Returns a user by id in admin update model format, or null if it does not exist.
        /// </summary>
        public async Task<AdminUpdateModel?> GetAdminUpdateModelById(int id)
        {
            User? user = await _userRepository.GetUserById(id);
            if (user == null)
                return null;

            AdminUpdateModel adminUpdateModel = new AdminUpdateModel
            {
                Username = user.Username,
                Fullname = user.Fullname,
                Role = Enum.Parse<UserRole>(user.Role)
            };
            return adminUpdateModel;
        }

        /// <summary>
        /// Updates a user by id, including its role, and returns it in DTO format, or the validation errors.
        /// </summary>
        public async Task<ServiceResult<UserDto>> UpdateUserByAdmin(AdminUpdateModel adminUpdateModel, int id)
        {
            ValidationContext context = new ValidationContext(adminUpdateModel);
            var result = new ServiceResult<UserDto>();

            if (!Validator.TryValidateObject(adminUpdateModel, context, result.Errors, true))
                return result;

            User? findUser = await _userRepository.GetUserById(id);
            if (findUser == null)
            {
                result.Errors.Add(new ValidationResult(UserModelRessources.UserNotFound));
                return result;
            }

            User? sameUsername = await _userRepository.GetUserByUsername(adminUpdateModel.Username);
            if (sameUsername != null && sameUsername.Id != id)
            {
                result.Errors.Add(new ValidationResult(UserModelRessources.UsernameUnavailable));
                return result;
            }

            findUser.Username = adminUpdateModel.Username;
            findUser.Fullname = adminUpdateModel.Fullname;
            findUser.Role = adminUpdateModel.Role.ToString();

            await _userRepository.UpdateUser(findUser);

            result.Data = MappingUserToDto(findUser);
            return result;
        }

        /// <summary>
        /// Creates a user with the default role and returns it in DTO format, or the validation errors.
        /// </summary>
        public async Task<ServiceResult<UserDto>> AddUser(UserSignUpModel userSignUpModel)
        {
            AdminSignUpModel adminSignUpModel = new AdminSignUpModel
            {
                Username = userSignUpModel.Username,
                Password = userSignUpModel.Password,
                Fullname = userSignUpModel.Fullname,
                Role = UserRole.User
            };
            return await AddUserByAdmin(adminSignUpModel);
        }

        /// <summary>
        /// Returns a user by id in user update model format, or null if it does not exist.
        /// </summary>
        public async Task<UserUpdateModel?> GetUserUpdateModelById(int id)
        {
            User? user = await _userRepository.GetUserById(id);
            if (user == null)
                return null;

            UserUpdateModel userUpdateModel = new UserUpdateModel
            {
                Username = user.Username,
                Fullname = user.Fullname
            };
            return userUpdateModel;
        }

        /// <summary>
        /// Updates a user by id, keeping its current role, and returns it in DTO format, or the validation errors.
        /// </summary>
        public async Task<ServiceResult<UserDto>> UpdateUser(UserUpdateModel userUpdateModel, int id)
        {
            User? findUser = await _userRepository.GetUserById(id);
            if (findUser == null)
            {
                var result = new ServiceResult<UserDto>();
                result.Errors.Add(new ValidationResult(UserModelRessources.UserNotFound));
                return result;
            }
            AdminUpdateModel adminUpdateModel = new AdminUpdateModel
            {
                Username = userUpdateModel.Username,
                Fullname = userUpdateModel.Fullname,
                Role = Enum.Parse<UserRole>(findUser.Role)
            };

            return await UpdateUserByAdmin(adminUpdateModel, id);
        }

        /// <summary>
        /// Changes the password of a user by id, after checking the old one, and returns the user in DTO format, or the errors.
        /// </summary>
        public async Task<ServiceResult<UserDto>> ChangePassword(ChangePasswordModel changePasswordModel, int id)
        {
            var result = new ServiceResult<UserDto>();
            ValidationContext context = new ValidationContext(changePasswordModel);

            if (!Validator.TryValidateObject(changePasswordModel, context, result.Errors, true))
                return result;

            User? findUser = await _userRepository.GetUserById(id);
            if (findUser == null)
            {
                result.Errors.Add(new ValidationResult(UserModelRessources.UserNotFound));
                return result;
            }

            var verifyOldPassword = _passwordHasher.VerifyHashedPassword(findUser, findUser.Password, changePasswordModel.OldPassword);
            if (verifyOldPassword == PasswordVerificationResult.Failed)
            {
                result.Errors.Add(new ValidationResult(UserModelRessources.OldPasswordIncorrect));
                return result;
            }

            findUser.Password = _passwordHasher.HashPassword(findUser, changePasswordModel.NewPassword);
            await _userRepository.UpdateUser(findUser);

            result.Data = MappingUserToDto(findUser);
            return result;
        }

        /// <summary>
        /// Deletes a user by id and returns an empty list, or an error if the user does not exist.
        /// </summary>
        public async Task<List<ValidationResult>> DeleteUserByAdmin(int id)
        {
            List<ValidationResult> result = new List<ValidationResult>();

            User? findUser = await _userRepository.GetUserById(id);
            if (findUser == null)
            {
                result.Add(new ValidationResult(UserModelRessources.UserNotFound));
                return result;
            }

            await _userRepository.DeleteUser(findUser);
            return result;
        }

        /// <summary>
        /// Returns a JWT token if the credentials are valid, otherwise null.
        /// </summary>
        public async Task<string?> Login(LoginModel loginModel)
        {
            User? user = await _userRepository.GetUserByUsername(loginModel.Username);
            if (user == null)
                return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, loginModel.Password);
            if (result == PasswordVerificationResult.Failed)
                return null;

            return _tokenService.GenerateToken(user);
        }

        ///////////////////// PRIVATE FUNCTION /////////////////////

        private UserDto MappingUserToDto(User user)
        {
            UserDto userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Fullname = user.Fullname,
                Role = Enum.Parse<UserRole>(user.Role)
            };

            return userDto;
        }


    }
}
