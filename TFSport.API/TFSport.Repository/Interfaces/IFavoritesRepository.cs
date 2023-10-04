using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSport.Models.Entities;

namespace TFSport.Repository.Interfaces
{
    public interface IFavoritesRepository
    {
        public Task<Favorites> GetById(string id);
        public Task UpdateAsync(Favorites favorites);
        public Task<List<Favorites>> GetFavoritesWithArticle(string id);
    }
}
