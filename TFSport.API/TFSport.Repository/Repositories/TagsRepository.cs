using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Extensions;
using TFSport.Models.Entities;
using TFSport.Repository.Interfaces;

namespace TFSport.Repository.Repositories
{
    public class TagsRepository : ITagsRepository
    {
        private readonly IRepository<Tag> _repository;

        public TagsRepository(IRepository<Tag> repository)
        {
            _repository = repository;
        }

        public async Task<Tag> GetTagAsync(string tagName)
        {
            return await _repository.GetAsync(x => x.Tags == tagName).FirstOrDefaultAsync();
        }

        public async Task<List<Tag>> GetTagsAsync(List<string> tagNames)
        {
            var tags = await _repository.GetAsync(x => tagNames.Contains(x.Tags));
            return tags.ToList();
        }

        public async Task CreateTagAsync(Tag tag)
        {
            await _repository.CreateAsync(tag);
        }

        public async Task UpdateTagAsync(Tag tag)
        {
            await _repository.UpdateAsync(tag);
        }

        public async Task DeleteTagAsync(string tagId)
        {
            await _repository.DeleteAsync(tagId);
        }
    }
}
