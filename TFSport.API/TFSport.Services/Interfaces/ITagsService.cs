using TFSport.Models.DTOs;
using TFSport.Models.Entities;

namespace TFSport.Services.Interfaces
{
    public interface ITagsService
    {
        public Task<IEnumerable<TagDto>> GetTopTagsAsync(int pageNumber, int pageSize);

        public Task CreateOrUpdateTagsAsync(HashSet<string> tagNames, string articleId, ArticleStatus articleStatus);

        public Task RemoveArticleTagsAsync(HashSet<string> tagNames, string articleId, ArticleStatus articleStatus);
    }
}
