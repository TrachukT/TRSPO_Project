using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TFSport.Exceptions;
using TFSport.Models;
using TFSport.Services;
using TFSport.Services.Interfaces;

namespace TFSport.API.Controllers
{
    [ApiController]
    [Route("users/tokens")]
    [Authorize]
    public class TokenController : ControllerBase
    {
        private readonly IJWTService _jwtService;

        public TokenController(IJWTService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("access-token")]
        public async Task<IActionResult> GetToken([FromBody] UserLogin model)
        {
            var email = model.Email.ToLower();
            var roles = new List<UserRoles> { UserRoles.User }; // Assuming user has a single role

            var token = await _jwtService.GenerateAccessTokenAsync(email, roles);

            return Ok(new { AccessToken = token });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = await _jwtService.GenerateRefreshTokenAsync();

            return Ok(new { RefreshToken = refreshToken });
        }
    }
}
