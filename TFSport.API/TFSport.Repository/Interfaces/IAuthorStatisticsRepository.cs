using System.Linq.Expressions;
using TFSport.Models.Entities;

namespace TFSport.Repository.Interfaces
{
    public interface IAuthorStatisticsRepository
    {
        public Task<AuthorStatistics> GetAuthorStatisticsAsync(string authorId);

        public Task<IEnumerable<AuthorStatistics>> GetAuthorsPageAsync(Expression<Func<AuthorStatistics, bool>> predicate, int pageNumber, int pageSize);

        public Task CreateAuthorStatisticsAsync(string authorId);

        public Task UpdateAuthorStatisticsAsync(string authorId, int articleCount);
    }
}
