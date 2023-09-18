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
            return await _repository.GetAsync(x => x.TagName == tagName).FirstOrDefaultAsync();
        }

        public async Task<List<Tag>> GetTagsAsync(List<string> tagNames)
        {
            var tags = await _repository.GetAsync(x => tagNames.Contains(x.TagName));
            return tags.ToList();
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            var tags = await _repository.GetAsync(_ => true);
            return tags;
        }

        public async Task<List<Tag>> GetTagsMatchingSubstringAsync(string substring)
        {
            var matchingTags = await _repository.GetAsync(x => x.TagName.Contains(substring));
            return matchingTags.ToList();
        }

        public async Task CreateTagAsync(Tag tag)
        {
            tag.PartitionKey = tag.Id;
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
