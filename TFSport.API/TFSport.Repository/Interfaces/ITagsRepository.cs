using TFSport.Models.Entities;

namespace TFSport.Repository.Interfaces
{
    public interface ITagsRepository
    {
        public Task<Tag> GetTagAsync(string tagName);

        public Task<HashSet<Tag>> GetTagsAsync(HashSet<string> tagNames);

        public Task<IEnumerable<Tag>> GetAllTagsAsync();

        public Task CreateTagAsync(Tag tag);

        public Task UpdateTagAsync(Tag tag);

        public Task DeleteTagAsync(string tagId);
    }
}
