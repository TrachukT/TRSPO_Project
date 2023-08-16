using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TFSport.API.Filters;
using TFSport.Models;
using TFSport.Services.Interfaces;

namespace TFSport.API.Controllers
{
    [ApiController]
    [Route("api/token")]
    public class TokenController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJWTService _jwtService;
        public TokenController(IUserService userService, IJWTService jwtService)
        {
            _userService = userService;
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
        /// <returns>A new access token and refresh token.</returns>
        [HttpPost]
        public async Task<IActionResult> GetTokens([FromBody] UserLoginDTO model)
        {
            var email = model.Email.ToLower();

            var isCredentialsValid = await _userService.ValidateCredentialsAsync(email, model.Password);
            if (!isCredentialsValid)
            {
                return BadRequest(ErrorMessages.InvalidCredentials);
            }

            var accessToken = await _jwtService.GenerateAccessTokenAsync(email);

            var refreshToken = await _jwtService.GenerateRefreshTokenAsync(email);

            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        /// <summary>
        /// Generates a new access token using a refresh token.
        /// </summary>
        /// <remarks>
        /// Sample request for refreshing an access token:
        /// <para>{
        ///     "refreshToken": "your-refresh-token"
        /// }</para>
        /// </remarks>
        /// <param name="model">The refresh token.</param>
        /// <returns>A new access token and refresh token.</returns>
        [HttpPost("refresh")]
        [RoleAuthorization(UserRoles.User, UserRoles.SuperAdmin)]
        public async Task<IActionResult> RefreshTokens([FromBody] RefreshRequestModel model)
        {
            var refreshToken = model.RefreshToken;

            try
            {
                var storedEmail = await _jwtService.GetEmailFromToken(refreshToken);

                if (!string.IsNullOrEmpty(storedEmail))
                {
                    var newAccessToken = await _jwtService.GenerateAccessTokenAsync(storedEmail);
                    var newRefreshToken = await _jwtService.GenerateRefreshTokenAsync(storedEmail);

                    return Ok(new
                    {
                        AccessToken = newAccessToken,
                        RefreshToken = newRefreshToken
                    });
                }
                else
                {
                    return BadRequest(ErrorMessages.InvalidRefreshToken);
                }
            }
            catch (Exception)
            {
                return BadRequest(ErrorMessages.InvalidRefreshToken);
            }
        }
    }
}
