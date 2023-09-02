using TFSport.API.DTOModels.Articles;
using TFSport.Models;

namespace TFSport.Services.Interfaces
{
	public interface IArticleService
	{
		public Task<List<ArticlesListModel>> ArticlesForApprove();

		public Task<List<ArticlesListModel>> AuthorsArticles(string authorId);
		
		public Task<List<ArticlesListModel>> PublishedArticles();

		public Task<List<ArticlesListModel>> MapArticles(List<Article> articles);

		public Task<Article> CreateArticleAsync(Article article, string content);

		public Task<string> GetArticleContentAsync(string articleId);

		public Task<Article> GetArticleWithContentByIdAsync(string articleId);

		public Task<Article> UpdateArticleAsync(Article article, string content);

		public Task DeleteArticleAsync(string articleId);

		public Task ChangeArticleStatusToReviewAsync(string articleId);

		public Task ChangeArticleStatusToPublishedAsync(string articleId);
    }
}
