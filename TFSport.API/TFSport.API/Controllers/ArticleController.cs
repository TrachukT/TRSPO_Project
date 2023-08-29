using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
using TFSport.API.Filters;
using TFSport.Services.Interfaces;

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
		
	}
}
