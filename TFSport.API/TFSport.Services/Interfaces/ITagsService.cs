namespace TFSport.Services.Interfaces
{
    public interface ITagsService
    {
        public Task CreateNewTagsAsync(List<string> tagNames, string articleId);

        public Task UpdateExistingTagsAsync(List<string> tagNames, string articleId);

        public Task RemoveArticleTagsAsync(List<string> tagNames, string articleId);
    }
}
