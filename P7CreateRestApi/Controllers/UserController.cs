using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Models.UserModel;
using P7CreateRestApi.Ressource;
using P7CreateRestApi.Service.Interfaces;
using System.Security.Claims;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Id of the connected user
        private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        /// <summary>
        /// GET: Returns the connected user in DTO format.
        /// </summary>
        [HttpGet("Profile")]
        public async Task<IActionResult> GetProfile()
        {
            UserDto? userDto = await _userService.GetUserById(CurrentUserId);
            if (userDto == null)
                return NotFound(new { message = UserModelRessources.UserNotFound });

            return Ok(userDto);
        }

        /// <summary>
        /// POST: Creates a user with the default role and returns it in DTO format.
        /// No authentication required.
        /// </summary>
        [HttpPost("Creation")]
        [AllowAnonymous]
        public async Task<IActionResult> AddNewUser([FromBody] UserSignUpModel userSignUpModel)
        {
            var result = await _userService.AddUser(userSignUpModel);
            if (result.Errors.Any())
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }

        /// <summary>
        /// GET: Returns the connected user in model format for update form.
        /// </summary>
        [HttpGet("FormUpdate")]
        public async Task<IActionResult> ShowUpdateForm()
        {
            UserUpdateModel? userUpdateModel = await _userService.GetUserUpdateModelById(CurrentUserId);
            if (userUpdateModel == null)
                return NotFound(new { message = UserModelRessources.UserNotFound });

            return Ok(userUpdateModel);
        }

        /// <summary>
        /// PUT: Updates the connected user, keeping its role, and returns it in DTO format.
        /// </summary>
        [HttpPut("Modification")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateModel userUpdateModel)
        {
            var result = await _userService.UpdateUser(userUpdateModel, CurrentUserId);
            if (result.Errors.Any())
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }

        /// <summary>
        /// PUT: Changes the password of the connected user, after checking the old one.
        /// </summary>
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel changePasswordModel)
        {
            var result = await _userService.ChangePassword(changePasswordModel, CurrentUserId);
            if (result.Errors.Any())
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }
    }
}