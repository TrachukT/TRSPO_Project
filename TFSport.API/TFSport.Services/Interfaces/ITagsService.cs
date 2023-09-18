using TFSport.Models.DTOs;
using TFSport.Models.Entities;

namespace TFSport.Services.Interfaces
{
    public interface ITagsService
    {
        public Task<IEnumerable<TagDto>> GetTopTagsAsync();

        public Task CreateOrUpdateTagsAsync(HashSet<string> tagNames, string articleId);

        public Task RemoveArticleTagsAsync(HashSet<string> tagNames, string articleId);
    }
}
