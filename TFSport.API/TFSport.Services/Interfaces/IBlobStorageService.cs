namespace TFSport.Services.Interfaces
{
    public interface IBlobStorageService
    {
        public Task UploadHtmlContentAsync(string containerName, string id, string htmlContent);

        public Task<string> GetHtmlContentAsync(string containerName, string articleId);

        public Task DeleteHtmlContentAsync(string containerName, string articleId);
    }
}
