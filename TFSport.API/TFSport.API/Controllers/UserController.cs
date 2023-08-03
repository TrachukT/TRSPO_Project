using Microsoft.AspNetCore.Mvc;
using TFSport.Models;
using TFSport.Services;

namespace TFSport.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            var registeredUser = await _userService.RegisterUser(user);
            return Ok(registeredUser);
        }
    }
}
