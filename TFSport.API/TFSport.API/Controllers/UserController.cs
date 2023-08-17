using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using TFSport.API.DTOModels.Users;
using TFSport.API.Filters;
using TFSport.Models;
using TFSport.Services.Interfaces;
using TFSport.Services.Services;

namespace TFSport.API.Controllers
{
	[ApiController]
	[Route("api/users")]

    [SwaggerResponse(400, "Bad_Request", typeof(string))]
    [SwaggerResponse(500, "Internal_Server_Error", typeof(string))]
    public class UserController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IJWTService _jwtService;
		private readonly IMapper _mapper;

		public UserController(IUserService userService, IMapper mapper, IJWTService jWTService)
		{
			_userService = userService;
			_mapper = mapper;
			_jwtService = jWTService;
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
		public async Task<IActionResult> Register([FromBody] UserRegisterDTO user)
		{
			try
			{
				await _userService.RegisterUser(_mapper.Map<User>(user));
				return Ok();
			}
			catch (ArgumentException arg)
			{
				return BadRequest(arg.Message);
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
        public async Task<IActionResult> ForgetPassword([FromBody][EmailAddress(ErrorMessage = ErrorMessages.EmailNotValid)] string email)
		{
			try
			{
				await _userService.ForgotPassword(email);
				return Ok();
			}
			catch (ArgumentException arg)
			{
				return BadRequest(arg.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		/// <summary>
		/// Restore password
		/// </summary>
		/// <param name="verificationToken"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		[HttpPost("recover-password")]
		[SwaggerResponse(200, "Request_Succeeded", typeof(string))]
        public async Task<IActionResult> RestorePassword([FromQuery] string verificationToken,[FromBody] RestorePasswordDTO password)
		{
			try
			{
				await _userService.RestorePassword(verificationToken, password.Password);
				return Ok();
			}
			catch (ArgumentException arg)
			{
				return BadRequest(arg.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost("confirmation")]
		[SwaggerResponse(200, "Request_Succeeded", typeof(string))]
		public async Task<IActionResult> EmailVerification([FromQuery] string verificationToken)
		{
			try
			{
				await _userService.EmailVerification(verificationToken);
				return Ok();
			}
			catch(ArgumentException arg)
			{
				return BadRequest(arg.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		/// <summary>
		/// Get all users info
		/// </summary>
		/// <returns></returns>
        [RoleAuthorization(UserRoles.SuperAdmin)]
		[HttpGet()]
		[SwaggerResponse(200, "Request_Succeeded", typeof(List<GetAllUsersDTO>))]
		[SwaggerResponse(400, "Bad_Request", typeof(string))]
		[SwaggerResponse(500, "Internal_Server_Error", typeof(string))]
		public async Task<IActionResult> GetAllUsers()
		{
			try
			{
				var users = await _userService.GetAllUsers();
				List<GetAllUsersDTO> list = _mapper.Map<List<GetAllUsersDTO>>(users);
				return Ok(list);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		/// <summary>
		/// Changes the role of a user based on the provided user ID and new role.
		/// </summary>
		/// <remarks>
		/// Sample request for changing user role:
		/// <code>
		/// {
		///     "newUserRole": "Author"
		/// }
		/// </code>
		/// </remarks>
		/// <param name="id">The ID of the user to change the role for.</param>
		/// <param name="request">The request object containing the new role.</param>
		/// <returns>A message indicating the result of the role change.</returns>
		[HttpPatch("{id}/role")]
		[SwaggerResponse(200, "Request_Succeeded", typeof(string))]
		[RoleAuthorization(UserRoles.SuperAdmin)]
		public async Task<IActionResult> ChangeUserRole(string id, [FromBody] ChangeUserRoleDTO request)
		{
			try
			{
				var newUserRole = request.NewUserRole;

				var userExists = await _userService.ChangeUserRole(id, newUserRole);

				if (userExists)
				{
					return Ok(new { Message = $"User with ID {id} has been granted a role {newUserRole}." });
				}
				else
				{
					return NotFound(ErrorMessages.UserNotFound);
				}
			}
			catch (ArgumentException arg)
			{
				return BadRequest(arg.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[RoleAuthorization(UserRoles.SuperAdmin, UserRoles.Author, UserRoles.User)]
		[HttpGet("/user")]
		[SwaggerResponse(200, "Request_Succeeded", typeof(User))]
		[SwaggerResponse(400, "Bad_Request", typeof(string))]
		[SwaggerResponse(500, "Internal_Server_Error", typeof(string))]
		public async Task<IActionResult> GetUserById()
		{
			try
			{
				HttpContext.Request.Headers.TryGetValue("Authorization", out var authToken);
				if (!authToken.ToString().StartsWith("Bearer ", ignoreCase: true, CultureInfo.CurrentCulture))
				{
					return BadRequest("Invalid token format");
				}

				var token = authToken.ToString().Replace("Bearer ", "", ignoreCase: true, CultureInfo.CurrentCulture);
				var id = await _jwtService.GetIdFromToken(token);
				if (id == null)
				{
					return BadRequest("Invalid token");
				}
				var user = await _userService.GetUserById(id);
				return Ok(user);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}
