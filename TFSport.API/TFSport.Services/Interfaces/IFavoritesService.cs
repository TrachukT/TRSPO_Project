using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSport.Models.DTOModels.Articles;
using TFSport.Models.Entities;

namespace TFSport.Services.Interfaces
{
    public interface IFavoritesService
    {
        public Task AddFavorite(string userId, string articleId);
        public Task RemoveFavorite(string userId, string articleId);
        public Task<List<ArticlesListModel>> GetFavorites(string id, int pageNumber, int pageSize, string orderBy, string order);
        public Task<Favorites> FindFavorires(string userId);
    }
}
