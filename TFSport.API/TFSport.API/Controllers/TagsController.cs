using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TFSport.API.Filters;
using TFSport.Models.DTOs;
using TFSport.Services.Interfaces;

namespace TFSport.API.Controllers
{
    [ApiController]
    [Route("tag")]
    [CustomExceptionFilter]
    [SwaggerResponse(400, "Bad_Request", typeof(string))]
    [SwaggerResponse(500, "Internal_Server_Error", typeof(string))]
    public class TagsController : ControllerBase
    {
        private readonly ITagsService _tagsService;

        public TagsController(ITagsService tagsService)
        {
            _tagsService = tagsService;
        }

        /// <summary>
        /// Get the top tags sorted by usage count.
        /// </summary>
        /// <returns>A list of top tags.</returns>
        [HttpGet("top")]
        [SwaggerResponse(200, "Request_Succeeded", typeof(IEnumerable<TagDto>))]
        public async Task<IActionResult> GetTopTags()
        {
            var topTags = await _tagsService.GetTopTagsAsync();
            return Ok(topTags);
        }
    }
}
