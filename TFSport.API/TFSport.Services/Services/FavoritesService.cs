using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSport.Models.DTOModels.Articles;
using TFSport.Models.Entities;
using TFSport.Models.Exceptions;
using TFSport.Repository.Interfaces;
using TFSport.Repository.Repositories;
using TFSport.Services.Interfaces;
using static System.Collections.Specialized.BitVector32;

namespace TFSport.Services.Services
{
    public class FavoritesService : IFavoritesService
    {
        private readonly IFavoritesRepository _favoritesRepository;
        private readonly IArticleService _articleService;
        private readonly IArticlesRepository _articleRepository;

        public FavoritesService(IFavoritesRepository favoritesRepository, IArticleService articleService, IArticlesRepository articleRepository)
        {
            _favoritesRepository = favoritesRepository;
            _articleService = articleService;
            _articleRepository = articleRepository;
        }

        public async Task<List<ArticlesListModel>> GetFavorites(string id, int pageNumber, int pageSize, string orderBy, string order)
        {
            try
            {
                var user = await _favoritesRepository.GetById(id);
                if (user == null)
                {
                    return new List<ArticlesListModel>();
                }
                var articles = new List<Article>();
                foreach (var articleId in user.FavoriteArticles)
                {
                    var article = await _articleRepository.GetArticleByIdAsync(articleId);
                    articles.Add(article);
                }

                return await _articleService.MapArticles(articles);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task AddFavorite(string userId, string articleId)
        {
            try
            {
                var userFavorites = await FindFavorires(userId);
                userFavorites.FavoriteArticles.Add(articleId);
                await _favoritesRepository.UpdateAsync(userFavorites);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task RemoveFavorite(string userId, string articleId)
        {
            var userFavorites =await FindFavorires(userId);
            userFavorites.FavoriteArticles.Remove(articleId);
            await _favoritesRepository.UpdateAsync(userFavorites);
        }
        
        public async Task<Favorites> FindFavorires(string userId)
        {
            var userFavorites = await _favoritesRepository.GetById(userId);
            if (userFavorites == null)
            {
                userFavorites = new Favorites
                {
                    UserId = userId,
                    FavoriteArticles = new HashSet<string>()
                };
            }
            return userFavorites;
        }
    }
}
