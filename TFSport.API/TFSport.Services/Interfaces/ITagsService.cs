namespace TFSport.Services.Interfaces
{
    public interface ITagsService
    {
        public Task CreateOrUpdateTagsAsync(HashSet<string> tagNames, string articleId);

        public Task RemoveArticleTagsAsync(HashSet<string> tagNames, string articleId);
    }
}
