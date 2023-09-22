using TFSport.Models.Entities;

namespace TFSport.Repository.Interfaces
{
    public interface IAuthorStatisticsRepository
    {
        public Task<AuthorStatistics> GetAuthorStatisticsAsync(string authorId);

        public Task<IEnumerable<AuthorStatistics>> GetAllAuthorsAsync();

        public Task CreateAuthorStatisticsAsync(string authorId);

        public Task UpdateAuthorStatisticsAsync(string authorId, int articleCount);
    }
}
