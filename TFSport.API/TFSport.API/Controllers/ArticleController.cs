using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
using TFSport.API.Filters;
using TFSport.Services.Interfaces;
using System.Security.Claims;
using TFSport.Models.Entities;
using TFSport.Models.DTOModels.Articles;

namespace TFSport.API.Controllers
{
    [Route("articles")]
    [ApiController]
    [CustomExceptionFilter]
    [SwaggerResponse(400, "Bad_Request", typeof(string))]
    [SwaggerResponse(500, "Internal_Server_Error", typeof(string))]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        /// <summary>
        /// Retrieves articles by tag with paging and ordering.
        /// </summary>
        /// <param name="tagName">The name of the tag to search for.</param>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The page size for pagination.</param>
        /// <param name="orderBy">The ordering criteria (e.g., "byCreatedDateAsc", "topRated").</param>
        /// <returns>A list of paged and ordered articles associated with the specified tag.</returns>
        [HttpGet("filter-by-tag")]
        [SwaggerResponse(200, "Request_Succeeded", typeof(OrderedArticlesDTO))]
        public async Task<IActionResult> GetArticlesByTag([FromQuery] string tagName, int pageNumber, int pageSize, string orderBy)
        {
            var articles = await _articleService.GetArticlesByTagAsync(tagName, pageNumber, pageSize, orderBy);
            return Ok(articles);
        }

        /// <summary>
        /// Retrieves articles by matching tags containing a specified substring.
        /// </summary>
        /// <param name="substring">The substring to search for in tags.</param>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The page size for pagination.</param>
        /// <param name="orderBy">The ordering criteria (e.g., "byCreatedDateAsc", "topRated").</param>
        /// <returns>A list of articles associated with matching tags.</returns>
        [HttpGet("search-by-tag")]
        [SwaggerResponse(200, "Request_Succeeded", typeof(OrderedArticlesDTO))]
        public async Task<IActionResult> SearchArticlesByTags([FromQuery] string substring, int pageNumber, int pageSize, string orderBy)
        {
            var articles = await _articleService.SearchArticlesByTagsAsync(substring, pageNumber, pageSize, orderBy);
            return Ok(articles);
        }

        /// <summary>
        /// Retrieves articles by matching titles containing a specified substring.
        /// </summary>
        /// <param name="substring">The substring to search for in titles.</param>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The page size for pagination.</param>
        /// <param name="orderBy">The ordering criteria (e.g., "byCreatedDateAsc", "topRated").</param>
        /// <returns>A list of articles associated with matching titles.</returns>
        [HttpGet("search-by-title")]
        [SwaggerResponse(200, "Request_Succeeded", typeof(OrderedArticlesDTO))]
        public async Task<IActionResult> SearchArticlesByTitleSubstring([FromQuery] string substring, int pageNumber, int pageSize, string orderBy)
        {
            var orderedArticles = await _articleService.SearchArticlesByTitleAsync(substring, pageNumber, pageSize, orderBy);
            return Ok(orderedArticles);
        }

        /// <summary>
        /// Retrieves articles by the author's full name.
        /// </summary>
        /// <param name="authorName">The full name of the author to search for.</param>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The page size for pagination.</param>
        /// <param name="orderBy">The ordering criteria (e.g., "byCreatedDateAsc", "topRated").</param>
        /// <returns>A list of articles associated with the specified author.</returns>
        [HttpGet("search-by-author")]
        [SwaggerResponse(200, "Request_Succeeded", typeof(IEnumerable<ArticleWithContentDTO>))]
        public async Task<IActionResult> SearchArticlesByAuthorName([FromQuery] string authorName, int pageNumber, int pageSize, string orderBy)
        {
            var articles = await _articleService.SearchArticlesByAuthorNameAsync(authorName, pageNumber, pageSize, orderBy);
            return Ok(articles);
        }

        /// <summary>
        /// Retrieves articles that are in the "Review" status.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The page size for pagination.</param>
        /// <param name="orderBy">The ordering criteria (e.g., "byCreatedDateAsc", "topRated").</param>
        /// <returns>A list of articles in "Review" status.</returns>
        [HttpGet("in-review")]
        [RoleAuthorization(UserRoles.SuperAdmin)]
        [SwaggerResponse(200, "Request_Succeeded", typeof(ArticlesListModel))]
        public async Task<IActionResult> GetArticlesForApprove(int pageNumber, int pageSize, string orderBy)
        {
            var articles = await _articleService.ArticlesForApprove(pageNumber, pageSize, orderBy);
            return Ok(articles);
        }

        /// <summary>
        /// Retrieves articles authored by the currently authenticated user.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The page size for pagination.</param>
        /// <param name="orderBy">The ordering criteria (e.g., "byCreatedDateAsc", "topRated").</param>
        /// <returns>A list of articles authored by the user.</returns>
        [HttpGet("mine")]
        [RoleAuthorization(UserRoles.Author)]
        [SwaggerResponse(200, "Request_Succeeded", typeof(ArticlesListModel))]
        public async Task<IActionResult> GetAuthorArticles(int pageNumber, int pageSize, string orderBy)
        {
            var authorId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var articles = await _articleService.AuthorsArticles(pageNumber, pageSize, orderBy,authorId);
            return Ok(articles);
        }

        /// <summary>
        /// Retrieves articles that are in the "Published" status.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The page size for pagination.</param>
        /// <param name="orderBy">The ordering criteria (e.g., "byCreatedDateAsc", "topRated").</param>
        /// <returns>list of articles that are in the "Published" status</returns>
        [HttpGet("published")]
        [SwaggerResponse(200, "Request_Succeeded", typeof(OrderedArticlesDTO))]
        public async Task<IActionResult> GetPublishedArticles([FromQuery] int pageNumber, int pageSize, string orderBy)
        {
            var articles = await _articleService.PublishedArticles(pageNumber,pageSize,orderBy);
            return Ok(articles);
        }

        /// <summary>
        /// Retrieves an article along with its HTML content.
        /// </summary>
        /// <param name="articleId">The ID of the article to retrieve.</param>
        /// <returns>The article information along with HTML content.</returns>
        [HttpGet("{articleId}")]
        [SwaggerResponse(200, "Request_Succeeded", typeof(ArticleWithContentDTO))]
        public async Task<IActionResult> GetArticleWithContent(string articleId)
        {
            var article = await _articleService.GetArticleWithContentByIdAsync(articleId);
            return Ok(article);
        }

        /// <summary>
        /// Creates a new article.
        /// </summary>
        /// <remarks>
        /// Sample request for creating an article:
        /// <code>
        /// {
        ///  "title": "Sample Article with a long title",
        ///  "sport": "Football",
        ///  "description": "Article description.",
        ///  "author": "345935d2-eddb-4f06-9170-37b930637751",
        ///  "image": "http://localhost:5293/images/Untitled.png",
        ///  "tags": [
        ///    "tag1"
        ///  ],
        ///  "content": "This is the content of the article. This is the content of the article. This is the content of the article. This is the content of the article. This is the content of the article. This is the content of the article. This is the content of the article. This is the content of the article. This is the content of the article. This is the content of the article. This is the content of the article."
        /// }
        /// </code>
        /// </remarks>
        /// <param name="request">The request object containing article information.</param>
        /// <returns>A message indicating the result of the article creation.</returns>
        [HttpPost]
        [RoleAuthorization(UserRoles.Author)]
        [SwaggerResponse(200, "Request_Succeeded")]
        public async Task<IActionResult> CreateArticle([FromBody] ArticleCreateDTO request)
        {
            await _articleService.CreateArticleAsync(request);
            return Ok();
        }

        /// <summary>
        /// Updates an existing article.
        /// </summary>
        /// <remarks>
        /// Sample request for updating an article:
        /// <code>
        /// {
        ///     "title": "Updated Article with a long title",
        ///     "sport": "Football",
        ///     "description": "Updated description.",
        ///     "image": "http://localhost:5293/images/8.png",
        ///     "tags":  [
        ///       "tag2"
        ///     ],
        ///     "content": "This is the updated content of the article. This is the updated content of the article. This is the updated content of the article. This is the updated content of the article. This is the updated content of the article. This is the updated content of the article. This is the updated content of the article. This is the updated content of the article. This is the updated content of the article. This is the updated content of the article."
        /// }
        /// </code>
        /// </remarks>
        /// <param name="articleId">The ID of the article to update.</param>
        /// <param name="updateDTO">The request object containing article updates.</param>
        /// <returns>A message indicating the result of the article update.</returns>
        [HttpPatch("{articleId}")]
        [RoleAuthorization(UserRoles.Author)]
        [SwaggerResponse(200, "Request_Succeeded")]
        public async Task<IActionResult> UpdateArticle(string articleId, [FromBody] ArticleUpdateDTO updateDTO)
        {
            string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _articleService.UpdateArticleAsync(articleId, updateDTO, userId);
            return Ok();
        }

        /// <summary>
        /// Deletes an article based on the provided article ID.
        /// </summary>
        /// <param name="articleId">The ID of the article to delete.</param>
        /// <returns>A message indicating the result of the article deletion.</returns>
        [HttpDelete("{articleId}")]
        [RoleAuthorization(UserRoles.Author, UserRoles.SuperAdmin)]
        [SwaggerResponse(200, "Request_Succeeded")]
        public async Task<IActionResult> DeleteArticle(string articleId)
        {
            await _articleService.DeleteArticleAsync(articleId);
            return Ok();
        }

        /// <summary>
        /// Changes the status of an article to "Published" for publication.
        /// </summary>
        /// <param name="articleId">The ID of the article to publish.</param>
        /// <returns>A message indicating the result of the status change.</returns>
        [HttpPatch("{articleId}/publish")]
        [RoleAuthorization(UserRoles.SuperAdmin)]
        [SwaggerResponse(200, "Request_Succeeded")]
        public async Task<IActionResult> ChangeArticleStatusToPublished(string articleId)
        {
            await _articleService.ChangeArticleStatusToPublishedAsync(articleId);
            return Ok();
        }

        /// <summary>
        /// Get list of user favorites(list of articles)
        /// </summary>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The page size for pagination.</param>
        /// <param name="orderBy">The ordering criteria (e.g., "byCreatedDateAsc", "topRated").</param>
        /// <returns></returns>
        [HttpGet("favorites")]
        [RoleAuthorization(UserRoles.SuperAdmin, UserRoles.User, UserRoles.Author)]
        [SwaggerResponse(200, "Request_Succeeded", typeof(OrderedArticlesDTO))]
        public async Task<IActionResult> GetMyFavorites([FromQuery] int pageNumber, int pageSize, string orderBy)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var articles = await _articleService.GetFavoriteArticles(pageNumber, pageSize, orderBy, userId);
            return Ok(articles);
        }

        /// <summary>
        /// Gets articles with choosed sport type
        /// </summary>
        /// <param name="sportType"> Choose one type from list</param>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The page size for pagination.</param>
        /// <param name="orderBy">The ordering criteria (e.g., "byCreatedDateAsc", "topRated").</param>
        /// <returns></returns>
        [HttpGet("filter-by-sport")]
        [SwaggerResponse(200, "Request_Succeeded", typeof(OrderedArticlesDTO))]
        public async Task<IActionResult> FilterBySportType([FromQuery] SportType sportType, int pageNumber, int pageSize, string orderBy)
        {
            var orderedArticles = await _articleService.FilterBySport(sportType, pageNumber, pageSize, orderBy);
            return Ok(orderedArticles);
        }

        /// <summary>
        /// Gets articles with choosed author
        /// </summary>
        /// <param name="authorId">Id of author</param>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The page size for pagination.</param>
        /// <param name="orderBy">The ordering criteria (e.g., "byCreatedDateAsc", "topRated").<param>
        /// <returns></returns>
        [HttpGet("filter-by-author")]
        [SwaggerResponse(200, "Request_Succeeded", typeof(OrderedArticlesDTO))]
        public async Task<IActionResult> FilterByAuthor([FromQuery] string authorId, int pageNumber, int pageSize, string orderBy)
        {
            var orderedArticles = await _articleService.FilterByAuthor(authorId, pageNumber, pageSize, orderBy);
            return Ok(orderedArticles);
        }

    }
}
