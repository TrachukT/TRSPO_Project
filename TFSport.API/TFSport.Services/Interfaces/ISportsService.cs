using TFSport.Models.DTOModels.Articles;
using TFSport.Models.Entities;

namespace TFSport.Services.Interfaces
{
    public interface ISportsService
    {
        public Task<List<SportsDTO>> GetSportTypes();

        public string GetDescription(SportType GenericEnum);
    }
}
