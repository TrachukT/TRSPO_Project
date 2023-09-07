﻿using Microsoft.AspNetCore.Http;
using TFSport.Models.DTOModels.Articles;
using TFSport.Models.Entities;

namespace TFSport.Services.Interfaces
{
    public interface IArticleService
	{
		public Task<List<ArticlesListModel>> ArticlesForApprove();

		public Task<List<ArticlesListModel>> AuthorsArticles(string authorId);
		
		public Task<List<ArticlesListModel>> PublishedArticles();

        public Task<ArticleWithContentDTO> GetArticleWithContentByIdAsync(string articleId);

        public Task<List<ArticlesListModel>> MapArticles(List<Article> articles);

		public Task CreateArticleAsync(ArticleCreateDTO articleDTO);

		public Task<Article> UpdateArticleAsync(string articleId, ArticleUpdateDTO articleUpdateDTO);

        public Task DeleteArticleAsync(string articleId);

		public Task ChangeArticleStatusToReviewAsync(string articleId);

		public Task ChangeArticleStatusToPublishedAsync(string articleId);
    }
}
