using TFSport.Models;
using TFSport.Models.DTOModels.Articles;
using TFSport.Models.Entities;

namespace TFSport.Services.Interfaces
{
    public interface IArticleService
    {
        public Task<OrderedArticlesDTO> ArticlesForApprove(int pageNumber, int pageSize, string orderBy);

        public Task<OrderedArticlesDTO> AuthorsArticles(int pageNumber, int pageSize, string orderBy, string authorId);

        public Task<OrderedArticlesDTO> PublishedArticles(int pageNumber, int pageSize, string orderBy);

        public Task<OrderedArticlesDTO> GetArticlesByTagAsync(string tagName, int pageNumber, int pageSize, string orderBy);

        public Task<OrderedArticlesDTO> SearchArticlesByTagsAsync(string substring, int pageNumber, int pageSize, string orderBy);

        public Task<OrderedArticlesDTO> SearchArticlesByTitleAsync(string substring, int pageNumber, int pageSize, string orderBy);

        public Task<OrderedArticlesDTO> SearchArticlesByAuthorNameAsync(string authorName, int pageNumber, int pageSize, string orderBy);

        public Task<OrderedArticlesDTO> FilterBySport(SportType sportType, int pageNumber, int pageSize, string orderBy);

        public Task<OrderedArticlesDTO> FilterByAuthor(string authorId, int pageNumber, int pageSize, string orderBy);

        public Task<ArticleWithContentDTO> GetArticleWithContentByIdAsync(string articleId);

        public Task<List<ArticlesListModel>> MapArticles(List<Article> articles);

        public Task<ArticleWithContentDTO> MapArticleWithContentAsync(Article article);

        public Task CreateArticleAsync(ArticleCreateDTO articleDTO);

        public Task<Article> UpdateArticleAsync(string articleId, ArticleUpdateDTO articleUpdateDTO, string userId);

        public Task DeleteArticleAsync(string articleId);

        public Task ChangeArticleStatusToPublishedAsync(string articleId);

        public Task<OrderedArticlesDTO> GetFavoriteArticles(int pageNumber, int pageSize, string orderBy, string userId);
    }
}
