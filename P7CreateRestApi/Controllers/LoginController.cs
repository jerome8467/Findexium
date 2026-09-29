using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Models.UserModel;
using P7CreateRestApi.Service.Interfaces;
namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {

        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("Access")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            string? token = await _userService.Login(loginModel);
            if (token == null)
                return Unauthorized();
            return Ok(new { token });
        }            
    }
}