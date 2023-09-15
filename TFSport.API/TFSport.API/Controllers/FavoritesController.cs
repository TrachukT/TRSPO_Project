using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
using System.Security.Claims;
using TFSport.API.Filters;
using TFSport.Models.Entities;
using TFSport.Repository.Interfaces;
using TFSport.Services.Interfaces;
using TFSport.Services.Services;

namespace TFSport.API.Controllers
{
    [Route("favorites")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoritesService _favoritesService;
        public FavoritesController(IFavoritesService favoritesService)
        {
            _favoritesService = favoritesService;
        }

        [HttpPost("add")]
        [RoleAuthorization(UserRoles.SuperAdmin, UserRoles.User, UserRoles.Author)]
        [SwaggerResponse(200, "Request_Succeeded")]
        public async Task<IActionResult> AddFavorite([FromBody] string articleId)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await _favoritesService.AddFavorite(userId, articleId);
            return Ok();
        }

        [HttpDelete("remove")]
        [RoleAuthorization(UserRoles.SuperAdmin, UserRoles.User, UserRoles.Author)]
        [SwaggerResponse(200, "Request_Succeeded")]
        public async Task<IActionResult> RemoveFavorite([FromBody] string articleId)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await _favoritesService.RemoveFavorite(userId, articleId);
            return Ok();
        }


    }
}
