using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IConfiguration _configuration;

        public ArticleController(IArticleService articleService, IMapper mapper, IBlobStorageService blobStorageService, IConfiguration configuration)
        {
            _articleService = articleService;
            _mapper = mapper;
            _blobStorageService = blobStorageService;
            _configuration = configuration;
        }

        /// <summary>
        /// Creates a new article.
        /// </summary>
        /// <remarks>
        /// Sample request for creating an article:
        /// <code>
        /// {
        ///     "title": "Sample Article",
        ///     "description": "Sample description",
        ///     "author": "ff9ce560-5dc7-4234-80d7-23c0ae39af66",
        ///     "content": "This is the content of the article."
        /// }
        /// </code>
        /// </remarks>
        /// <param name="request">The request object containing article information.</param>
        /// <returns>A message indicating the result of the article creation.</returns>
        [HttpPost]
        [RoleAuthorization(UserRoles.Author, UserRoles.SuperAdmin)]
        [SwaggerResponse(200, "Request_Succeeded")]
        public async Task<IActionResult> CreateArticle([FromBody] ArticleCreateDTO request)
        {
			await _articleService.CreateArticleAsync(_mapper.Map<Article>(request), request.Content);
            return Ok();
        }

        /// <summary>
        /// Retrieves articles that are in the "Review" status.
        /// </summary>
        /// <returns>A list of articles in "Review" status.</returns>
        [HttpGet("in-review")]
		[RoleAuthorization(UserRoles.SuperAdmin)]
		[SwaggerResponse(200, "Request_Succeeded", typeof(ArticlesListModel))]
		public async Task<IActionResult> GetArticlesForApprove()
		{
			var articles = await _articleService.ArticlesForApprove();
			return Ok(articles);
		}

        /// <summary>
        /// Retrieves articles authored by the currently authenticated user.
        /// </summary>
        /// <returns>A list of articles authored by the user.</returns>
        [HttpGet("mine")]
        [RoleAuthorization(UserRoles.Author, UserRoles.SuperAdmin)]
        [SwaggerResponse(200, "Request_Succeeded", typeof(ArticlesListModel))]
		public async Task<IActionResult> GetAuthorArticles()
		{
			var authorId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
			var articles = await _articleService.AuthorsArticles(authorId);
			return Ok(articles);
		}

        /// <summary>
        /// Retrieves articles that are in the "Published" status.
        /// </summary>
        /// <returns>A list of articles in "Published" status.</returns>
        [HttpGet("published")]
		[SwaggerResponse(200, "Request_Succeeded", typeof(ArticlesListModel))]
		public async Task<IActionResult> GetPublishedArticles()
		{
			var articles = await _articleService.PublishedArticles();
			return Ok(articles);
		}

        /// <summary>
        /// Retrieves the HTML content of an article.
        /// </summary>
        /// <param name="articleId">The ID of the article to retrieve content for.</param>
        /// <returns>The HTML content of the article.</returns>
        [HttpGet("{articleId}/content")]
        [RoleAuthorization(UserRoles.Author, UserRoles.SuperAdmin)]
        [SwaggerResponse(200, "Request_Succeeded")]
        public async Task<IActionResult> GetArticleContent(string articleId)
        {
            var content = await _articleService.GetArticleContentAsync(articleId);
            return Ok(content);
        }

        /// <summary>
        /// Retrieves an article along with its HTML content.
        /// </summary>
        /// <param name="articleId">The ID of the article to retrieve.</param>
        /// <returns>The article information along with HTML content.</returns>
        [HttpGet("{articleId}")]
        [RoleAuthorization(UserRoles.Author, UserRoles.SuperAdmin)]
        [SwaggerResponse(200, "Request_Succeeded", typeof(GetArticleWithContentDTO))]
        public async Task<IActionResult> GetArticleWithContent(string articleId)
        {
            var article = await _articleService.GetArticleWithContentByIdAsync(articleId);

            var articleWithContent = _mapper.Map<GetArticleWithContentDTO>(article);

            articleWithContent.Content = await _blobStorageService.GetHtmlContentAsync(_configuration["BlobStorageContainers:ArticleContainer"], articleId);

            return Ok(articleWithContent);
        }

        /// <summary>
        /// Updates an existing article.
        /// </summary>
        /// <remarks>
        /// Sample request for updating an article:
        /// <code>
        /// {
        ///     "title": "Updated Article",
        ///     "description": "Updated description.",
        ///     "content": "This is the updated content of the article."
        /// }
        /// </code>
        /// </remarks>
        /// <param name="articleId">The ID of the article to update.</param>
        /// <param name="updateDTO">The request object containing article updates.</param>
        /// <returns>A message indicating the result of the article update.</returns>
        [HttpPatch("{articleId}")]
        [RoleAuthorization(UserRoles.Author, UserRoles.SuperAdmin)]
        [SwaggerResponse(200, "Request_Succeeded")]
        public async Task<IActionResult> UpdateArticle(string articleId, [FromBody] ArticleUpdateDTO updateDTO)
        {
            var existingArticle = await _articleService.GetArticleWithContentByIdAsync(articleId);

            _mapper.Map(updateDTO, existingArticle);

            await _articleService.UpdateArticleAsync(existingArticle, updateDTO.Content);

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
        /// Changes the status of an article to "Review" for review and approval.
        /// </summary>
        /// <param name="articleId">The ID of the article to send for review.</param>
        /// <returns>A message indicating the result of the status change.</returns>
        [HttpPatch("{articleId}/send-for-review")]
        [RoleAuthorization(UserRoles.Author, UserRoles.SuperAdmin)]
        [SwaggerResponse(200, "Request_Succeeded")]
        public async Task<IActionResult> ChangeArticleStatusToReview(string articleId)
        {
            await _articleService.ChangeArticleStatusToReviewAsync(articleId);
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
    }
}
