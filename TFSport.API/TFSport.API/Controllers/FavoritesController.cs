using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
using System.Security.Claims;
using TFSport.API.Filters;
using TFSport.Models.Entities;
using TFSport.Services.Interfaces;

namespace TFSport.API.Controllers
{
    [Route("favorites")]
    [CustomExceptionFilter]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoritesService _favoritesService;
        public FavoritesController(IFavoritesService favoritesService)
        {
            _favoritesService = favoritesService;
        }

        /// <summary>
        /// Adds an article to a user's favorites.
        /// </summary>
        /// <param name="articleId">The ID of the article to add to favorites.</param>
        /// <remarks>
        /// Sample request for adding an article to favorites:
        /// <code>
        /// "articleId"
        /// </code>
        /// </remarks>
        /// <returns>A message indicating the result of adding the article to favorites.</returns>
        [HttpPost("add")]
        [RoleAuthorization(UserRoles.SuperAdmin, UserRoles.User, UserRoles.Author)]
        [SwaggerResponse(200, "Request_Succeeded")]
        public async Task<IActionResult> AddFavorite([FromBody] string articleId)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await _favoritesService.AddFavorite(userId, articleId);
            return Ok();
        }

        /// <summary>
        /// Removes an article from a user's favorites.
        /// </summary>
        /// <param name="articleId">The ID of the article to remove from favorites.</param>
        /// <remarks>
        /// Sample request for removing an article from favorites:
        /// <code>
        /// "articleId"
        /// </code>
        /// </remarks>
        /// <returns>A message indicating the result of removing the article from favorites.</returns>
        [HttpDelete("remove")]
        [RoleAuthorization(UserRoles.SuperAdmin, UserRoles.User, UserRoles.Author)]
        [SwaggerResponse(200, "Request_Succeeded")]
        public async Task<IActionResult> RemoveFavorite([FromBody] string articleId)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await _favoritesService.RemoveFavorite(userId, articleId);
            return Ok();
        }

        /// <summary>
        /// Gets list of favorites(id`s)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorization(UserRoles.SuperAdmin, UserRoles.User, UserRoles.Author)]
        [SwaggerResponse(200, "Request_Succeeded", typeof(HashSet<string>))]
        public async Task<IActionResult> GetMyFavorites()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var articles = await _favoritesService.GetFavoritesIDs(userId);
            return Ok(articles);
        }
    }
}
