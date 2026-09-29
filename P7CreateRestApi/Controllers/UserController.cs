using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Models.UserModel;
using P7CreateRestApi.Ressource;
using P7CreateRestApi.Service.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET : All Users
        [HttpGet("list")]
        public async Task<IActionResult> GetAllUsers()
        {
            IEnumerable<UserDto> usersDtos = await _userService.GetAllUser();

            return Ok(usersDtos);
        }

        // GET : User By Username
        [HttpGet("DetailsByUsername/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            UserDto usersDto = await _userService.GetUserByUsername(username);
            if (usersDto == null)
                return NotFound(new { message = UserModelRessources.UserNotFound });

            return Ok(usersDto);
        }

        // GET : User By ID
        [HttpGet("DetailsById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            UserDto usersDto = await _userService.GetUserById(id);
            if(usersDto == null)
                return NotFound(new {message = UserModelRessources.UserNotFound});

            return Ok(usersDto);
        }

        // POST : New User
        [HttpPost("Creation")]
        public async Task<IActionResult> AddNewUser(SignUpModel signUpModel)
        {
            var result = await _userService.AddUser(signUpModel);
            if(result.Errors.Any())
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }

        // GET : User by ID for UpdateForm
        [HttpGet("FormUpdate/{id}")]
        public async Task<IActionResult> ShowUpdateForm(int id)
        {
            UpdateUserModel? updateUserModel = await _userService.GetUpdateUserModelById(id);
            if(updateUserModel == null)
                return NotFound(new { message = UserModelRessources.UserNotFound });

            return Ok(updateUserModel);
        }

        // PUT : Update User with UpdateUserModel
        [HttpPut("Modification/{id}")]
        public async Task<IActionResult> UpdateUser(UpdateUserModel updateUserModel, int id)
        {
            var result = await _userService.UpdateUser(updateUserModel, id);
            if (result.Errors.Any())
                return BadRequest(result.Errors);
            return Ok(result.Data);
        }

        // PUT : Change Password with old and new
        [HttpPut("ChangePassword/{id}")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePasswordModel, int id)
        {
            var result = await _userService.ChangePassword(changePasswordModel, id);
            if (result.Errors.Any())
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }

        // DELETE : Delete User by ID
        [HttpDelete("Removal/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            List<ValidationResult> errors = await _userService.DeleteUser(id);
            if (errors.Any())
                return NotFound(errors);
            return Ok();
        }

    }
}