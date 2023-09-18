using TFSport.Models;
using TFSport.Models.DTOModels.Articles;
using TFSport.Models.Entities;

namespace TFSport.Services.Interfaces
{
    public interface IArticleService
    {
        public Task<List<ArticlesListModel>> ArticlesForApprove();

        public Task<List<ArticlesListModel>> AuthorsArticles(string authorId);

        public Task<OrderedArticlesDTO> PublishedArticles(int pageNumber, int pageSize, string orderBy);

        public Task<IEnumerable<ArticleWithContentDTO>> GetArticlesByTagAsync(string tagName);

        public Task<IEnumerable<ArticleWithContentDTO>> SearchArticlesByTagsAsync(string substring);

        public Task<List<ArticleWithContentDTO>> GetArticlesWithContentByIdsAsync(IEnumerable<string> articleIds);

        public Task<ArticleWithContentDTO> GetArticleWithContentByIdAsync(string articleId);

        public Task<List<ArticlesListModel>> MapArticles(List<Article> articles);

        public Task<ArticleWithContentDTO> MapArticleWithContentAsync(Article article);

        public Task CreateArticleAsync(ArticleCreateDTO articleDTO);

        public Task<Article> UpdateArticleAsync(string articleId, ArticleUpdateDTO articleUpdateDTO, string userId);

        public Task DeleteArticleAsync(string articleId);

        public Task ChangeArticleStatusToReviewAsync(string articleId, string userId);

		public Task ChangeArticleStatusToPublishedAsync(string articleId);
    }
}
