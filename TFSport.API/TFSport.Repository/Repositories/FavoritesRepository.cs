using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSport.Models.Entities;
using TFSport.Models.Exceptions;
using TFSport.Repository.Interfaces;

namespace TFSport.Repository.Repositories
{
    public class FavoritesRepository : IFavoritesRepository
    {
        private readonly IRepository<Favorites> _repository;

        public FavoritesRepository(IRepository<Favorites> repository)
        {
            _repository = repository;
        }

        public async Task<Favorites> GetById(string id)
        {
            var favorites = await _repository.GetAsync(x=>x.UserId == id).FirstOrDefaultAsync();
            return favorites;
        }

        public async Task UpdateAsync(Favorites favorites)
        {
            await _repository.UpdateAsync(favorites, default);
        }
    }
}
