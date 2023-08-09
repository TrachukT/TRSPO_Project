using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TFSport.Models;
using TFSport.Services.Interfaces;

namespace TFSport.API.Controllers
{
    [ApiController]
    [Route("token")]
    [Authorize]
    public class TokenController : ControllerBase
    {
        private readonly IJWTService _jwtService;

        public TokenController(IJWTService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost]
        public async Task<IActionResult> GetToken([FromBody] UserLogin model)
        {
            var email = model.Email.ToLower();

            var token = await _jwtService.GenerateAccessTokenAsync(email);

            return Ok(new { AccessToken = token });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = await _jwtService.GenerateRefreshTokenAsync();

            return Ok(new { RefreshToken = refreshToken });
        }
    }
}
