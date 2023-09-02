using Azure.Storage.Blobs;
using TFSport.Models;
using TFSport.Services.Interfaces;

namespace TFSport.Services.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task UploadHtmlContentAsync(string containerName, string id, string htmlContent)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                await containerClient.CreateIfNotExistsAsync();

                var blobClient = containerClient.GetBlobClient(id);

                using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(htmlContent));
                await blobClient.UploadAsync(stream, true);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<string> GetHtmlContentAsync(string containerName, string articleId)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

                if (!await containerClient.ExistsAsync())
                {
                    throw new CustomException(ErrorMessages.BlobContainerDoesntExist);
                }

                var blobClient = containerClient.GetBlobClient(articleId);

                if (!await blobClient.ExistsAsync())
                {
                    throw new CustomException(ErrorMessages.BlobDoesntExist);
                }

                using var response = await blobClient.OpenReadAsync();
                using var reader = new StreamReader(response);
                var content = await reader.ReadToEndAsync();

                return content;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task DeleteHtmlContentAsync(string containerName, string articleId)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

                if (!await containerClient.ExistsAsync())
                {
                    throw new CustomException(ErrorMessages.BlobContainerDoesntExist);
                }

                var blobClient = containerClient.GetBlobClient(articleId);

                if (!await blobClient.ExistsAsync())
                {
                    throw new CustomException(ErrorMessages.BlobDoesntExist);
                }

                await blobClient.DeleteIfExistsAsync();
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
    }
}
