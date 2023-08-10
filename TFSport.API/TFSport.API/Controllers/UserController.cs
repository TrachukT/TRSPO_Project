using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using TFSport.API.DTOModels.Users;
using TFSport.Models;
using TFSport.Services.Interfaces;

namespace TFSport.API.Controllers
{
	[ApiController]
	[Route("api/users")]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IJWTService _JWTService;
		private readonly IMapper _mapper;

		public UserController(IUserService userService, IMapper mapper, IJWTService jWTService)
		{
			_userService = userService;
			_mapper = mapper;
			_JWTService = jWTService;
		}

		/// <summary>
		/// Main registration form
		/// </summary>
		/// <remarks>
		/// Sample request for registration form:
		/// <para>{</para>
		///  <para>"firstName": "Olena",</para>
		///  <para>"lastName": "Lingan",</para>
		///  <para>"email": "user@gmail.com",</para>
		///  <para>"password": "Kh0ajf81!l_",</para>
		///  <para>"repeatPassword": "Kh0ajf81!l_"</para>
		///	<para>}</para>
		/// </remarks>
		/// <param name="user"></param>
		/// <returns></returns>
		[HttpPost("register")]
		[SwaggerResponse(200, "Request_Succeeded", typeof(UserRegisterDTO))]
		[SwaggerResponse(400, "Bad_Request", typeof(string))]
		[SwaggerResponse(500, "Internal_Server_Error", typeof(string))]
		public async Task<IActionResult> Register([FromBody] UserRegisterDTO user)
		{
			try
			{
				await _userService.RegisterUser(_mapper.Map<User>(user));
				return Ok();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		/// <summary>
		/// Pop-up window "Forgot password"
		/// </summary>
		/// <param name="email"></param>
		/// <returns></returns>
		[HttpPost("restore-password")]
		[SwaggerResponse(200, "Request_Succeeded", typeof(string))]
		[SwaggerResponse(400, "Bad_Request", typeof(string))]
		[SwaggerResponse(500, "Internal_Server_Error", typeof(string))]
        public async Task<IActionResult> ForgetPassword([FromBody][EmailAddress(ErrorMessage = ErrorMessages.EmailNotValid)] string email)
		{
			try
			{
				await _userService.ForgotPassword(email);
				return Ok();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		/// <summary>
		/// Update password(email link)
		/// </summary>
		/// <param name="password"></param>
		/// <returns></returns>
		[HttpPut("password")]
		[SwaggerResponse(200, "Request_Succeeded", typeof(string))]
		[SwaggerResponse(400, "Bad_Request", typeof(string))]
		[SwaggerResponse(500, "Internal_Server_Error", typeof(string))]
        public async Task<IActionResult> RestorePassword([FromBody] RestorePasswordDTO password)
		{
			try
			{
				await _userService.RestorePassword(password.VerificationToken, password.Password);
				return Ok();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize]
		[HttpPost("email-verification")]
		[SwaggerResponse(200, "Request_Succeeded", typeof(string))]
		[SwaggerResponse(400, "Bad_Request", typeof(string))]
		[SwaggerResponse(500, "Internal_Server_Error", typeof(string))]
		public async Task<IActionResult> EmailVerification([FromQuery] string verificationToken)
		{
			try
			{
				HttpContext.Request.Headers.TryGetValue("Authorization", out var authToken);
				if (!authToken.ToString().StartsWith("Bearer ", ignoreCase: true, CultureInfo.CurrentCulture))
				{
					return BadRequest("Invalid token format");
				}

				var token = authToken.ToString().Replace("Bearer ", "", ignoreCase: true, CultureInfo.CurrentCulture);
				var email = await _JWTService.GetEmailFromToken(token);
				await _userService.EmailVerification(email, verificationToken);
				return Ok();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}
