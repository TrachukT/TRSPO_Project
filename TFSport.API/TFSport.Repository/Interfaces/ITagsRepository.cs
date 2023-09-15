using TFSport.Models.Entities;

namespace TFSport.Repository.Interfaces
{
    public interface ITagsRepository
    {
        public Task<Tag> GetTagAsync(string tagName);

        public Task<List<Tag>> GetTagsAsync(List<string> tagNames);

        public Task<List<Tag>> GetTagsMatchingSubstringAsync(string substring);

        public Task CreateTagAsync(Tag tag);

        public Task UpdateTagAsync(Tag tag);

        public Task DeleteTagAsync(string tagId);
    }
}
