using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Models.UserModel;
using P7CreateRestApi.Ressource;
using P7CreateRestApi.Service.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("[controller]/User")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// GET: Returns all users in DTO format.
        /// </summary>
        [HttpGet("List")]
        public async Task<IActionResult> GetAllUsers()
        {
            IEnumerable<UserDto> usersDtos = await _userService.GetAllUser();

            return Ok(usersDtos);
        }

        /// <summary>
        /// GET: Returns user by username in DTO format.
        /// </summary>
        [HttpGet("DetailsByUsername/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            UserDto? userDto = await _userService.GetUserByUsername(username);
            if(userDto == null)
                return NotFound(new { message = UserModelRessources.UserNotFound });

            return Ok(userDto);
        }

        /// <summary>
        /// GET: Returns user by id in DTO format.
        /// </summary>
        [HttpGet("DetailsById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            UserDto? userDto = await _userService.GetUserById(id);
            if(userDto == null)
                return NotFound(new {message = UserModelRessources.UserNotFound});

            return Ok(userDto);
        }

        /// <summary>
        /// POST: Creates user with chosen role and returns DTO format.
        /// </summary>
        [HttpPost("Creation")]
        public async Task<IActionResult> AddNewUser([FromBody] AdminSignUpModel adminSignUpModel)
        {
            var result = await _userService.AddUserByAdmin(adminSignUpModel);
            if(result.Errors.Any())
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }

        /// <summary>
        /// GET: Returns user by id in model format for update form.
        /// </summary>
        [HttpGet("FormUpdate/{id}")]
        public async Task<IActionResult> ShowUpdateForm(int id)
        {
            AdminUpdateModel? adminUpdateModel = await _userService.GetAdminUpdateModelById(id);
            if(adminUpdateModel == null)
                return NotFound(new { message = UserModelRessources.UserNotFound });

            return Ok(adminUpdateModel);
        }

        /// <summary>
        /// PUT: Updates user by id and returns DTO format.
        /// </summary>
        [HttpPut("Modification/{id}")]
        public async Task<IActionResult> UpdateUser(AdminUpdateModel adminUpdateModel, int id)
        {
            var result = await _userService.UpdateUserByAdmin(adminUpdateModel, id);
            if (result.Errors.Any())
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }

        /// <summary>
        /// DELETE: Deletes user by id.
        /// </summary>
        [HttpDelete("Removal/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            List<ValidationResult> errors = await _userService.DeleteUserByAdmin(id);
            if (errors.Any())
                return NotFound(errors);

            return Ok();
        }
    }
}