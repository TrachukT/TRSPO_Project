using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSport.Models;
using TFSport.Services.Interfaces;

namespace TFSport.Services.Services
{
	public class ArticleService : IArticleService
	{
		private readonly IRepository<Article> _articleRepo;

		public ArticleService(IRepository<Article> articleRepo)
		{
			_articleRepo = articleRepo;
		}

		public async Task<List<Article>> ArticlesForApprove()
		{
			try
			{
				var articles = await _articleRepo.GetAsync(x => x.Status == PostStatus.Review).ToListAsync();
				if (articles.Count == 0)
				{
					throw new CustomException(ErrorMessages.NoArticlesForReview);
				}
				return articles;
			}
			catch (Exception ex)
			{
				throw new CustomException(ex.Message);
			}
		}

		public async Task<List<Article>> AuthorsArticles(string authorId)
		{
			try
			{
				var articles = await _articleRepo.GetAsync(x => x.Author == authorId).ToListAsync();
				if (articles.Count == 0)
				{
					throw new CustomException(ErrorMessages.NoAuthorsArticles);
				}
				return articles;
			}
			catch (Exception ex)
			{
				throw new CustomException(ex.Message);
			}
		}

		public async Task<List<Article>> PublishedArticles()
		{
			try
			{
				var articles = await _articleRepo.GetAsync(x => x.Status == PostStatus.Published).ToListAsync();
				if (articles.Count == 0)
				{
					throw new CustomException(ErrorMessages.NoArticlesPublished);
				}
				return articles;
			}
			catch (Exception ex)
			{
				throw new CustomException(ex.Message);
			}
		}

		public async Task CreateArticle()
		{

		}

	}
}
