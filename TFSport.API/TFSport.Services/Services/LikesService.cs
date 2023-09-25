using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSport.Models.Entities;
using TFSport.Models.Exceptions;
using TFSport.Repository.Interfaces;
using TFSport.Services.Interfaces;

namespace TFSport.Services.Services
{
    public class LikesService : ILikesService
    {
        private readonly IArticlesRepository _articlesRepository;

        public LikesService(IArticlesRepository articlesRepository)
        {
            _articlesRepository = articlesRepository;
        }

        public async Task AddLikeInfo(string articleId, string userId)
        {
            try
            {
                var article = await _articlesRepository.GetArticleByIdAsync(articleId);
                if (article == null)
                {
                    throw new CustomException(ErrorMessages.ArticleDoesntExist);
                }
                article.LikeCount++;
                if (article.Likes == null)
                    article.Likes = new HashSet<string>();
                article.Likes.Add(userId);
                var id = await _articlesRepository.UpdateArticleAsync(article);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<List<string>> GetLikeInfo(string articleId)
        {
            try
            {
                var article = await _articlesRepository.GetLikedArticles(articleId);
                return article;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task RemoveLikeInfo(string articleId, string userId)
        {
            try
            {
                var article = await _articlesRepository.GetArticleByIdAsync(articleId);
                if (article == null)
                {
                    throw new CustomException(ErrorMessages.ArticleDoesntExist);
                }
                article.LikeCount--;
                article.Likes.Remove(userId);
                var id = await _articlesRepository.UpdateArticleAsync(article);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
    }
}
