using P7CreateRestApi.Dto;
using P7CreateRestApi.Models.UserModel;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Service.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<UserDto>> GetAllUser();
        public Task<UserDto?> GetUserByUsername(string username);
        public Task<UserDto?> GetUserById(int id);
        public Task<ServiceResult<UserDto>> AddUserByAdmin(AdminSignUpModel adminSignUpModel);
        public Task<AdminUpdateModel?> GetAdminUpdateModelById(int id);
        public Task<ServiceResult<UserDto>> UpdateUserByAdmin(AdminUpdateModel adminUpdateModel, int id);
        public Task<ServiceResult<UserDto>> AddUser(UserSignUpModel userSignUpModel);
        public Task<UserUpdateModel?> GetUserUpdateModelById(int id);
        public Task<ServiceResult<UserDto>> UpdateUser(UserUpdateModel userUpdateModel, int id);
        public Task<ServiceResult<UserDto>> ChangePassword(ChangePasswordModel changePasswordModel, int id);
        public Task<List<ValidationResult>> DeleteUserByAdmin(int id);
        public Task<string?> Login(LoginModel loginmodel);
    }
}
