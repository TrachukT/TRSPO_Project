using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TFSport.API.Filters;
using TFSport.Models.DTOModels.Users;
using TFSport.Services.Interfaces;

namespace TFSport.API.Controllers
{
    [Route("authors")]
    [ApiController]
    [CustomExceptionFilter]
    [SwaggerResponse(400, "Bad_Request", typeof(string))]
    [SwaggerResponse(500, "Internal_Server_Error", typeof(string))]
    public class TopAuthorsController : ControllerBase
    {
        private readonly ITopAuthorsService _topAuthorService;

        public TopAuthorsController(ITopAuthorsService topAuthorService)
        {
            _topAuthorService = topAuthorService;
        }

        /// <summary>
        /// Get the top authors sorted by Published articles count.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The page size for pagination.</param>
        /// <returns>A list of top authors.</returns>
        [HttpGet("top")]
        [SwaggerResponse(200, "Request_Succeeded", typeof(IEnumerable<AuthorDTO>))]
        public async Task<IActionResult> GetTopAuthorsAsync([FromQuery] int pageNumber, int pageSize)
        {
            var authorDtos = await _topAuthorService.GetTopAuthorsAsync(pageNumber, pageSize);
            return Ok(authorDtos);
        }
    }
}
