using TFSport.Models.DTOModels.Users;

namespace TFSport.Services.Interfaces
{
    public interface ITopAuthorsService
    {
        public Task<IEnumerable<AuthorDTO>> GetTopAuthorsAsync(int pageNumber, int pageSize);
    }
}
