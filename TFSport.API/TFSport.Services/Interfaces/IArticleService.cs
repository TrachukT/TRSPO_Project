﻿using TFSport.Models;
using TFSport.Models.DTOModels.Articles;
using TFSport.Models.Entities;

namespace TFSport.Services.Interfaces
{
    public interface IArticleService
    {
        public Task<OrderedArticlesDTO> ArticlesForApprove(int pageNumber, int pageSize, string orderBy);

        public Task<OrderedArticlesDTO> AuthorsArticles(int pageNumber, int pageSize, string orderBy, string authorId);

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

        public Task ChangeArticleStatusToPublishedAsync(string articleId);

        public Task<OrderedArticlesDTO> GetFavoriteArticles(int pageNumber, int pageSize, string orderBy, string userId);
    }
}
