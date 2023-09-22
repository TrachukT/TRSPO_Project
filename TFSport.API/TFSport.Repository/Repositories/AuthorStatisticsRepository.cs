using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Extensions;
using TFSport.Models.Entities;
using TFSport.Repository.Interfaces;

namespace TFSport.Repository.Repositories
{
    public class AuthorStatisticsRepository : IAuthorStatisticsRepository
    {
        private readonly IRepository<AuthorStatistics> _repository;

        public AuthorStatisticsRepository(IRepository<AuthorStatistics> repository)
        {
            _repository = repository;
        }

        public async Task<AuthorStatistics> GetAuthorStatisticsAsync(string authorId)
        {
            var statistics = await _repository.GetAsync(x => x.AuthorId == authorId).FirstOrDefaultAsync();
            return statistics;
        }

        public async Task<IEnumerable<AuthorStatistics>> GetAllAuthorsAsync()
        {
            var authors = await _repository.GetAsync(_ => true);
            return authors;
        }

        public async Task CreateAuthorStatisticsAsync(string authorId)
        {
            var statistics = new AuthorStatistics { AuthorId = authorId, ArticleCount = 0 };
            statistics.PartitionKey = statistics.Id;
            await _repository.CreateAsync(statistics);
        }

        public async Task UpdateAuthorStatisticsAsync(string authorId, int articleCount)
        {
            var statistics = await _repository.GetAsync(x => x.AuthorId == authorId).FirstOrDefaultAsync();
            if (statistics != null)
            {
                statistics.ArticleCount += articleCount;
                await _repository.UpdateAsync(statistics);
            }
        }
    }
}
