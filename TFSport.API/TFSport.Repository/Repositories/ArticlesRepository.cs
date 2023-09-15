using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Extensions;
using Microsoft.Azure.CosmosRepository.Paging;
using System.Linq.Expressions;
using TFSport.Models;
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

        public async Task<List<Article>> GetArticlesInReview()
        {
            var articles = await _repository.GetAsync(x => x.Status == ArticleStatus.Review).ToListAsync();
            return articles;
        }

        public async Task<List<Article>> GetAuthorsArticles(string authorId)
        {
            var articles = await _repository.GetAsync(x => x.Author == authorId).ToListAsync();
            return articles;
        }

        public async Task<List<Article>> GetPublishedArticles()
        {
            var articles = await _repository.GetAsync(x => x.Status == ArticleStatus.Published).ToListAsync();
            return articles;
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

        public async Task ChangeArticleStatusToReviewAsync(Article article)
        {
            article.Status = ArticleStatus.Review;
            await _repository.UpdateAsync(article);
        }

        public async Task ChangeArticleStatusToPublishedAsync(Article article)
        {
            article.Status = ArticleStatus.Published;
            await _repository.UpdateAsync(article);
        }

        public async Task<List<Article>> GetArticles(int pageNumber, int pageSize, string orderBy, Expression<Func<Article, bool>> predicate = null, HashSet<string> articleIds = null)
        {
            IPageQueryResult<Article> articles = null;
            if (predicate != null)
            {
                articles = await _repository.PageAsync(predicate, pageNumber: pageNumber, pageSize: pageSize);
            }

            if(articleIds != null)
            {
                articles = await _repository.PageAsync(x => articleIds.Contains(x.Id), pageNumber: pageNumber, pageSize: pageSize);
            }

            List<Article> items = new List<Article>();
            switch (orderBy)
            {
                case OrderType.byCreatedDateDesc:
                    items = articles.Items.OrderByDescending(x => x.CreatedTimeUtc).ToList();
                    break;
                case OrderType.byCreatedDateAsc:
                    items = articles.Items.OrderBy(x => x.CreatedTimeUtc).ToList();
                    break;
                case OrderType.topRated:
                    items = articles.Items.OrderByDescending(x => x.LikeCount).ToList();
                    break;
                default:
                    break;
            }
            return items;
        }
    }
}
