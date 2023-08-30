using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
using TFSport.API.Filters;
using TFSport.Services.Interfaces;
using TFSport.Models;
using System.Security.Claims;
using TFSport.API.DTOModels.Articles;

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

		public ArticleController(IArticleService articleService, IMapper mapper)
		{
			_articleService = articleService;
			_mapper = mapper;
		}

		[HttpPost]		
		public async Task<IActionResult> CreateArticle()
		{
			await _articleService.CreateArticle();
			return Ok();
		}

		[HttpGet("review")]
		[RoleAuthorization(UserRoles.SuperAdmin)]
		[SwaggerResponse(200, "Request_Succeeded", typeof(GetArticlesListDTO))]
		public async Task<IActionResult> GetArticlesForApprove()
		{
			var articles = await _articleService.ArticlesForApprove();
			return Ok(articles);
		}

		[RoleAuthorization(UserRoles.Author,UserRoles.SuperAdmin)]
		[HttpGet("mine")]
		[SwaggerResponse(200, "Request_Succeeded", typeof(GetArticlesListDTO))]
		public async Task<IActionResult> GetAuthorArticles()
		{
			var authorId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
			var articles = await _articleService.AuthorsArticles(authorId);
			return Ok(articles);
		}

		[HttpGet]
		[SwaggerResponse(200, "Request_Succeeded", typeof(GetArticlesListDTO))]
		public async Task<IActionResult> GetPublishedArticles()
		{
			var articles = await _articleService.PublishedArticles();
			var list = new List<GetArticlesListDTO>();
			foreach(var article in articles)
			{
				list.Add(_mapper.Map<GetArticlesListDTO>(article));
			}
			return Ok(list);
		}
	}
}
