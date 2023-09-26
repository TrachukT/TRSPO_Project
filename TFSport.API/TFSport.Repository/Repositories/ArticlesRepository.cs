using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Extensions;
using System.Linq.Expressions;
using TFSport.Models.Entities;
using TFSport.Repository.Interfaces;

namespace TFSport.Repository.Repositories
{
    public class ArticlesRepository : IArticlesRepository
    {
        private readonly IRepository<Article> _repository;

        public ArticlesRepository(IRepository<Article> repository)
        {
            _repository = repository;
        }

        public async Task<int> GetCountofArticles(Expression<Func<Article, bool>> predicate)
        {
            var articles = await _repository.GetAsync(predicate).ToListAsync();
            return articles.Count;
        }

        public async Task<Article> GetArticleByTitleAsync(string title)
        {
            var article = await _repository.GetAsync(x => x.Title == title).FirstOrDefaultAsync();
            return article;
        }

        public async Task<Article> GetArticleByIdAsync(string articleId)
        {
            var article = await _repository.GetAsync(articleId);
            return article;
        }

        public async Task CreateArticleAsync(Article article)
        {
            await _repository.CreateAsync(article, default);
        }

        public async Task<Article> UpdateArticleAsync(Article article)
        {
            await _repository.UpdateAsync(article);
            var updatedArticle = await _repository.GetAsync(article.Id);
            return updatedArticle;
        }

        public async Task DeleteArticleAsync(Article article)
        {
            await _repository.DeleteAsync(article);
        }

        public async Task ChangeArticleStatusToPublishedAsync(Article article)
        {
            article.Status = ArticleStatus.Published;
            await _repository.UpdateAsync(article);
        }

        public async Task<IEnumerable<Article>> GetArticles(int pageNumber, int pageSize, string orderBy,
        Expression<Func<Article, bool>> predicate = null, HashSet<string> articleIds = null)
        {
            if (articleIds != null && articleIds.Count == 0)
                return new List<Article>();

            DefaultArticleSpecification specification = new(pageNumber, pageSize, orderBy, predicate, articleIds);
            var query = await _repository.QueryAsync(specification);
            return query.Items.ToList();
        }

        public async Task<List<string>> GetLikedArticles(string userId)
        {
            var articles = await _repository.GetAsync(x => x.LikedUserIds.Contains(userId));
            var list = articles.Select(x => x.Id).ToList();
            return list;
        }
    }
}
