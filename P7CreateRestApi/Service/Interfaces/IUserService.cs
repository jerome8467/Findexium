using P7CreateRestApi.Models.UserModel;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Service.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<UserDto>> GetAllUser();
        public Task<UserDto> GetUserByUsername(string username);
        public Task<UserDto> GetUserById(int id);
        public Task<ServiceResult<UserDto>> AddUser(SignUpModel signUpModel);
        public Task<UpdateUserModel> GetUpdateUserModelById(int id);
        public Task<ServiceResult<UserDto>> UpdateUser(UpdateUserModel updateUserModel, int id);
        public Task<ServiceResult<UserDto>> ChangePassword(ChangePasswordModel changePasswordModel, int id);
        public Task<List<ValidationResult>> DeleteUser(int id);
        public Task<string?> Login(LoginModel loginmodel);
    }
}
