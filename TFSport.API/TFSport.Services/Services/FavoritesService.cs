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

        public async Task ManageFavorites(string userId, string articleId, string action)
        {
            try
            {
                var userFavorites = await _favoritesRepository.GetById(userId);
                if (userFavorites == null)
                {
                    userFavorites = new Favorites
                    {
                        UserId = userId,
                        FavoriteArticles = new List<string>()
                    };
                }

                if (action.ToLower() == "add")
                    userFavorites.FavoriteArticles.Add(articleId);
                else
                    userFavorites.FavoriteArticles.Remove(articleId);

                await _favoritesRepository.UpdateAsync(userFavorites);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<List<ArticlesListModel>> GetFavorites(string id)
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
    }
}
