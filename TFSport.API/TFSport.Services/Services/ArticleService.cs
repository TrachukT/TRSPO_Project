using AutoMapper;
using TFSport.Models.DTOModels.Articles;
using TFSport.Models.DTOModels.Users;
using TFSport.Models.Entities;
using TFSport.Models.Exceptions;
using TFSport.Repository.Interfaces;
using TFSport.Services.Interfaces;

namespace TFSport.Services.Services
{
    public class ArticleService : IArticleService
	{
		private readonly IArticlesRepository _articleRepository;
		private readonly IUserService _userService;
		private readonly IMapper _mapper;

		public ArticleService(IArticlesRepository articleRepo, IUserService userService, IMapper mapper)
		{
			_articleRepository = articleRepo;
			_userService = userService;
			_mapper = mapper;
		}

		public async Task<List<ArticlesListModel>> ArticlesForApprove()
		{
			try
			{
				var articles = await _articleRepository.GetArticlesInReview();
				var list = await MapArticles(articles);
				return list;
			}
			catch (Exception ex)
			{
				throw new CustomException(ex.Message);
			}
		}

		public async Task<List<ArticlesListModel>> AuthorsArticles(string authorId)
		{
			try
			{
				var articles = await _articleRepository.GetAuthorsArticles(authorId);
				var list = await MapArticles(articles);
				return list;
			}
			catch (Exception ex)
			{
				throw new CustomException(ex.Message);
			}
		}

		public async Task<List<ArticlesListModel>> PublishedArticles()
		{
			try
			{
				var articles = await _articleRepository.GetPublishedArticles();
				var list = await MapArticles(articles);
				return list;
			}
			catch (Exception ex)
			{
				throw new CustomException(ex.Message);
			}
		}

		public async Task<List<ArticlesListModel>> MapArticles(List<Article> articles)
		{
            if (articles.Count == 0)
            {
                return new List<ArticlesListModel>();
            }
            var list = new List<ArticlesListModel>();
			foreach (var article in articles)
			{
				var user = await _userService.GetUserById(article.Author);
				var userDTO = _mapper.Map<UserInfo>(user);
				var articleDTO = _mapper.Map<ArticlesListModel>(article);
				articleDTO.Author = userDTO;
				list.Add(articleDTO);
			}
			return list;
		}
		public async Task CreateArticle()
		{

		}

	}
}
