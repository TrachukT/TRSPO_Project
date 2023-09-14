using TFSport.Models.Entities;

namespace TFSport.Services.Interfaces
{
    public interface ISportService
    {
        public Task<List<SportType>> GetSportTypes();
    }
}
