using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSport.Models.DTOModels.Articles;
using TFSport.Models.Entities;

namespace TFSport.Services.Interfaces
{
    public interface IArticleService
	{
		public Task CreateArticle();

		public Task<List<ArticlesListModel>> ArticlesForApprove();

		public Task<List<ArticlesListModel>> AuthorsArticles(string authorId);
		
		public Task<List<ArticlesListModel>> PublishedArticles();

		public Task<List<ArticlesListModel>> MapArticles(List<Article> articles);
	}
}
