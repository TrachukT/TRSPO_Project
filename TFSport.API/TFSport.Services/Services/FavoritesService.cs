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
                        FavoriteArticles = new HashSet<string>()
                    };
                    userFavorites.PartitionKey = userFavorites.Id;
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

        public async Task<HashSet<string>> GetFavorites(string id)
        {
            try
            {
                var user = await _favoritesRepository.GetById(id);
                if (user == null)
                {
                    return new HashSet<string>();
                }
                
                return user.FavoriteArticles;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
    }
}
