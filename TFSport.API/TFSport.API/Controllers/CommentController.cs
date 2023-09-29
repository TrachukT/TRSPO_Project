using Microsoft.AspNetCore.Mvc;
using TFSport.Services.Interfaces;
using TFSport.Models.DTOModels.Comments;
using TFSport.API.Filters;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using TFSport.Models.Entities;

namespace TFSport.Controllers
{
    [ApiController]
    [Route("comments")]
    [CustomExceptionFilter]
    [SwaggerResponse(400, "Bad_Request", typeof(string))]
    [SwaggerResponse(500, "Internal_Server_Error", typeof(string))]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        /// <summary>
        /// Adds a new comment to an article.
        /// </summary>
        /// <param name="commentDto">The comment data to add.</param>
        /// <param name="articleId">The ID of the article to add the comment to.</param>
        /// <returns>An HTTP response indicating success and the created comment as a CommentDTO.</returns>
        [HttpPost]
        [SwaggerResponse(200, "Request_Succeeded", typeof(CommentDTO))]
        [RoleAuthorization(UserRoles.SuperAdmin, UserRoles.Author, UserRoles.User)]
        public async Task<IActionResult> AddComment([FromBody] CommentCreateDTO commentDto, [FromQuery] string articleId)
        {
            var authorId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var createdComment = await _commentService.AddCommentAsync(commentDto, articleId, authorId);
            return Ok(createdComment);
        }

        /// <summary>
        /// Deletes a comment by its ID.
        /// </summary>
        /// <param name="commentId">The ID of the comment to delete.</param>
        /// <returns>An HTTP response indicating success.</returns>
        [HttpDelete("{commentId}")]
        [SwaggerResponse(200, "Request_Succeeded")]
        [RoleAuthorization(UserRoles.SuperAdmin, UserRoles.Author, UserRoles.User)]
        public async Task<IActionResult> DeleteComment(string commentId)
        {
            var authorId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            await _commentService.DeleteCommentAsync(commentId, authorId);
            return Ok();
        }

        /// <summary>
        /// Retrieves comments for an article by its ID with paging.
        /// </summary>
        /// <param name="articleId">The ID of the article to retrieve comments for.</param>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The page size for pagination.</param>
        /// <returns>A list of paged comments associated with the specified article.</returns>
        [HttpGet("{articleId}")]
        [SwaggerResponse(200, "Request_Succeeded", typeof(CommentListDTO))]
        public async Task<IActionResult> GetCommentsByArticleId(string articleId, [FromQuery] int pageNumber, int pageSize)
        {
            var comments = await _commentService.GetCommentsByArticleIdAsync(articleId, pageNumber, pageSize);
            return Ok(comments);
        }
    }
}