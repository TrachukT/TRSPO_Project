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

        public FavoritesService(IFavoritesRepository favoritesRepository)
        {
            _favoritesRepository = favoritesRepository;
        }

        public async Task<HashSet<string>> GetFavorites(string id, int pageNumber, int pageSize, string orderBy)
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

        public async Task AddFavorite(string userId, string articleId)
        {
            try
            {
                var userFavorites = await FindFavorites(userId);
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
            try
            {
                var userFavorites = await FindFavorites(userId);
                userFavorites.FavoriteArticles.Remove(articleId);
                await _favoritesRepository.UpdateAsync(userFavorites);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<Favorites> FindFavorites(string userId)
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
                }
                return userFavorites;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<HashSet<string>> GetFavoritesIDs(string userId)
        {
            try
            {
                var userFavorites = await _favoritesRepository.GetById(userId);
                if (userFavorites == null)
                {
                    return new HashSet<string>();
                }
                return userFavorites.FavoriteArticles;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
    }
}
