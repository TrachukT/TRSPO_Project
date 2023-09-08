using Microsoft.AspNetCore.Http;
using TFSport.Models.Entities;

namespace TFSport.Services.Interfaces
{
    public interface IBlobStorageService
    {
        public Task UploadHtmlContentAsync(string containerName, string id, string htmlContent);

        public Task<string> GetHtmlContentAsync(string containerName, string articleId);

        public Task DeleteHtmlContentAsync(string containerName, string articleId);

        public Task<BlobStorageFile> UploadImageAsync(Stream fileStream, string containerName, string fileName);

        public Task<BlobStorageFile> UploadImageAsync(IFormFile file, string containerName, string fileName);

        public Task<BlobStorageFile> UploadImage(byte[] fileBytes, string containerName, string fileName);

        public Task<Stream> GetImageAsync(string containerName, string imageName);
    }
}
