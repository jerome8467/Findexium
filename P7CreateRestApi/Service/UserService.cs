using Microsoft.AspNetCore.Identity;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Models.UserModel;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Ressource;
using P7CreateRestApi.Service.Interfaces;
using System.ComponentModel.DataAnnotations;

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

        public async Task<UserDto> GetUserByUsername(string username)
        {
            User? user = await _userRepository.GetUserByUsername(username);
            if (user == null) 
                return null;

            return MappingUserToDto(user);
        }

        public async Task<UserDto> GetUserById(int id)
        {
            User? user = await _userRepository.GetUserById(id);
            if (user == null)
                return null;

            return MappingUserToDto(user);
        }

        public async Task<UpdateUserModel> GetUpdateUserModelById(int id)
        {
            User? user = await _userRepository.GetUserById(id);
            if (user == null)
                return null;

            UpdateUserModel updateUserModel = new UpdateUserModel
            {
                Username = user.Username,
                Fullname = user.Fullname,
                Role = user.Role,
            };
            return updateUserModel;
        }

        public async Task<ServiceResult<UserDto>> AddUser(SignUpModel signUpModel)
        {
            ValidationContext context = new ValidationContext(signUpModel);
            var result = new ServiceResult<UserDto>();

            if (!Validator.TryValidateObject(signUpModel, context, result.Errors, true))
                return result;

            User? findUser = await _userRepository.GetUserByUsername(signUpModel.Username);
            if (findUser != null)
            {
                result.Errors.Add(new ValidationResult(UserModelRessources.UsernameUnavailable));
                return result;
            }

            User user = new User
            {
                Username = signUpModel.Username,
                Fullname = signUpModel.Fullname,
                Role = signUpModel.Role
            };
            user.Password = _passwordHasher.HashPassword(user, signUpModel.Password);

            await _userRepository.AddUser(user);

            result.Data = MappingUserToDto(user); ;
            return result;
        }

        public async Task<ServiceResult<UserDto>> UpdateUser(UpdateUserModel updateUserModel, int id)
        {
            ValidationContext context = new ValidationContext(updateUserModel);
            var result = new ServiceResult<UserDto>();

            if (!Validator.TryValidateObject(updateUserModel, context, result.Errors, true))
                return result;

            User? findUser = await _userRepository.GetUserByUsername(updateUserModel.Username);
            if (findUser != null && findUser.Id != id)
            {
                result.Errors.Add(new ValidationResult(UserModelRessources.UsernameUnavailable));
                return result;
            }

            User user = new User
            {
                Id = id,
                Username = updateUserModel.Username,
                Fullname = updateUserModel.Fullname,
                Role = updateUserModel.Role
            };

            bool success = await _userRepository.UpdateUser(user);
            if (!success)
            {
                result.Errors.Add(new ValidationResult(UserModelRessources.UserNotFound));
                return result;
            }

            result.Data = MappingUserToDto(user);
            return result;
        }

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

            string newPassowrd = _passwordHasher.HashPassword(findUser, changePasswordModel.NewPassword);
            await _userRepository.ChangePassword(newPassowrd, id);

            result.Data = MappingUserToDto(findUser);
            return result;
        }

        public async Task<bool> DeleteUser(int id)
        {
            return await _userRepository.DeleteUser(id);
        }

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

        private UserDto MappingUserToDto(User user)
        {
            UserDto userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Fullname = user.Fullname,
                Role = user.Role
            };

            return userDto;
        }


    }
}
