using Microsoft.Extensions.Caching.Memory;
using TFSport.Models.Entities;
using TFSport.Services.Interfaces;

namespace TFSport.Services.Services
{
    public class SportService : ISportService
    {
        private readonly IMemoryCache _memoryCache;

        public SportService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<List<SportType>> GetSportTypes()
        {
            var isCached = _memoryCache.TryGetValue(nameof(SportType), out List<SportType> sportsList);
            if (!isCached)
            {
                sportsList = new List<SportType>();
                var sportsValues = Enum.GetValues(typeof(SportType));
                foreach (var value in sportsValues)
                {
                    sportsList.Add((SportType)value);
                }
                _memoryCache.Set(nameof(SportType), sportsList, new MemoryCacheEntryOptions()
                    .SetSize(5)
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
            }
            return sportsList;
        }
    }
}
