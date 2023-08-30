using AutoMapper;
using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Extensions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSport.API.DTOModels.Articles;
using TFSport.API.DTOModels.Users;
using TFSport.Models;
using TFSport.Services.Interfaces;

namespace TFSport.Services.Services
{
	public class ArticleService : IArticleService
	{
		private readonly IRepository<Article> _articleRepo;
		private readonly IUserService _userService;
		private readonly IMapper _mapper;

		public ArticleService(IRepository<Article> articleRepo, IUserService userService, IMapper mapper)
		{
			_articleRepo = articleRepo;
			_userService = userService;
			_mapper = mapper;
		}

		public async Task<List<ArticlesListModel>> ArticlesForApprove()
		{
			try
			{
				var articles = await _articleRepo.GetAsync(x => x.Status == PostStatus.Review).ToListAsync();
				if (articles.Count == 0)
				{
					throw new CustomException(ErrorMessages.NoArticlesForReview);
				}
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
				var articles = await _articleRepo.GetAsync(x => x.Author == authorId).ToListAsync();
				if (articles.Count == 0)
				{
					throw new CustomException(ErrorMessages.NoAuthorsArticles);
				}
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
				var articles = await _articleRepo.GetAsync(x => x.Status == PostStatus.Published).ToListAsync();
				if (articles.Count == 0)
				{
					throw new CustomException(ErrorMessages.NoArticlesPublished);
				}
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
