using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TFSport.Models;
using TFSport.Services.Interfaces;

namespace TFSport.API.Controllers
{
    [ApiController]
    [Route("api/token")]
    public class TokenController : ControllerBase
    {
        private readonly IJWTService _jwtService;
        public TokenController(IJWTService jwtService)
        {
            _jwtService = jwtService;
        }

        /// <summary>
        /// Generates a new access token based on the provided user login credentials.
        /// </summary>
        /// <remarks>
        /// Sample request for generating an access token:
        /// <para>{
        ///     "email": "user@gmail.com",
        ///     "password": "password123"
        /// }</para>
        /// </remarks>
        /// <param name="model">The user's login credentials.</param>
        /// <returns>A new access token.</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GetToken([FromBody] UserLoginDTO model)
        {
            var email = model.Email.ToLower();

            var token = await _jwtService.GenerateAccessTokenAsync(email);

            return Ok(new { AccessToken = token });
        }

        /// <summary>
        /// Generates a new access token using a refresh token.
        /// </summary>
        /// <remarks>
        /// Sample request for refreshing an access token:
        /// <para>{
        ///     "refreshToken": "your-refresh-token",
        ///     "email": "user@gmail.com"
        /// }</para>
        /// </remarks>
        /// <param name="model">The refresh token and user's email.</param>
        /// <returns>A new access token.</returns>
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshRequestModel model)
        {
            var email = model.Email.ToLower();

            var newAccessToken = await _jwtService.GenerateAccessTokenAsync(email);

            return Ok(new { AccessToken = newAccessToken });
        }
    }
}
