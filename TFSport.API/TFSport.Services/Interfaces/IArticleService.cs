using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSport.Models;

namespace TFSport.Services.Interfaces
{
	public interface IArticleService
	{
		public Task CreateArticle();

		public Task<List<Article>> ArticlesForApprove();

		public Task<List<Article>> AuthorsArticles(string authorId);
		
		public Task<List<Article>> PublishedArticles();
	}
}
