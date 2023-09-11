using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSport.Models.DTOModels.Articles;

namespace TFSport.Services.Interfaces
{
    public interface IFavoritesService
    {
        public Task ManageFavorites(string userId, string articleId, string action);
        public Task<List<ArticlesListModel>> GetFavorites(string id);
    }
}
