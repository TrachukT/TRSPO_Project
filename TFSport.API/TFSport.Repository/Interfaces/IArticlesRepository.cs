using System.Linq.Expressions;
using TFSport.Models;
using TFSport.Models.Entities;

namespace TFSport.Repository.Interfaces
{
    public interface IArticlesRepository
    {
        public Task<int> GetCountofArticles(Expression<Func<Article, bool>> predicate);

        public Task<List<Article>> GetAuthorsArticles(string authorId);

        public Task<List<Article>> GetArticlesInReview();
        public Task<Article> GetArticleByTitleAsync(string title);

        public Task CreateArticleAsync(Article article);

        public Task<Article> GetArticleByIdAsync(string articleId);

        public Task<Article> UpdateArticleAsync(Article article);

        public Task DeleteArticleAsync(Article article);

        public Task ChangeArticleStatusToReviewAsync(Article article);

        public Task ChangeArticleStatusToPublishedAsync(Article article);

        public Task<IEnumerable<Article>> GetArticles(int pageNumber, int pageSize, string orderBy,
     Expression<Func<Article, bool>> predicate = null, HashSet<string> articleIds = null);
    }
}
