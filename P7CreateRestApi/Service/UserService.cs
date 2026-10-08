using Microsoft.AspNetCore.Identity;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Dto;
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

        public async Task<ServiceResult<UserDto>> UpdateUser(UpdateUserModel model, int id)
        {
            ValidationContext context = new ValidationContext(model);
            var result = new ServiceResult<UserDto>();

            if (!Validator.TryValidateObject(model, context, result.Errors, true))
                return result;

            User? findUser = await _userRepository.GetUserById(id);
            if (findUser == null)
            {
                result.Errors.Add(new ValidationResult(UserModelRessources.UserNotFound));
                return result;
            }

            User? sameUsername = await _userRepository.GetUserByUsername(model.Username);
            if (sameUsername != null && sameUsername.Id != id)
            {
                result.Errors.Add(new ValidationResult(UserModelRessources.UsernameUnavailable));
                return result;
            }

            findUser.Username = model.Username;
            findUser.Fullname = model.Fullname;
            findUser.Role = model.Role;

            await _userRepository.UpdateUser(findUser);

            result.Data = MappingUserToDto(findUser);
            return result;
        }

        public async Task<ServiceResult<UserDto>> ChangePassword(ChangePasswordModel changePassword, int id)
        {
            var result = new ServiceResult<UserDto>();
            ValidationContext context = new ValidationContext(changePassword);

            if (!Validator.TryValidateObject(changePassword, context, result.Errors, true))
                return result;

            User? findUser = await _userRepository.GetUserById(id);
            if (findUser == null)
            {
                result.Errors.Add(new ValidationResult(UserModelRessources.UserNotFound));
                return result;
            }

            var verifyOldPassword = _passwordHasher.VerifyHashedPassword(findUser, findUser.Password, changePassword.OldPassword);
            if (verifyOldPassword == PasswordVerificationResult.Failed)
            {
                result.Errors.Add(new ValidationResult(UserModelRessources.OldPasswordIncorrect));
                return result;
            }

            findUser.Password = _passwordHasher.HashPassword(findUser, changePassword.NewPassword);
            await _userRepository.UpdateUser(findUser);

            result.Data = MappingUserToDto(findUser);
            return result;
        }

        public async Task<List<ValidationResult>> DeleteUser(int id)
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
                Role = user.Role
            };

            return userDto;
        }


    }
}
