using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
using System.ComponentModel.DataAnnotations;
using TFSport.API.DTOModels.Users;
using TFSport.Models;
using TFSport.Services.Interfaces;
using TFSport.Services.Services;

namespace TFSport.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService,IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("register")]
		[SwaggerResponse(200, "Request_Succeeded", typeof(UserRegisterDTO))]
		[SwaggerResponse(400, "Bad_Request", typeof(string))]
		[SwaggerResponse(500, "Internal_Server_Error", typeof(string))]
		public async Task<IActionResult> Register([FromBody]UserRegisterDTO user)
        {
            try
            {
                var registeredUser = await _userService.RegisterUser(_mapper.Map<User>(user));
                return Ok();
            }
			catch (Exception ex)
			{
				return StatusCode(500,ex.Message);
			}
        }
		[HttpPost("restore-password")]
		[SwaggerResponse(200, "Request_Succeeded",typeof(string))]
		[SwaggerResponse(400, "Bad_Request", typeof(string))]
		[SwaggerResponse(500, "Internal_Server_Error", typeof(string))]
        public async Task<IActionResult> ForgetPassword([FromBody][EmailAddress(ErrorMessage =Errors.EmailNotValid)] string email)
        {
            return Ok();
        }
	}
}
