using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using TFSport.Models.Entities;
using TFSport.Models.Exceptions;
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

        public Task<BlobStorageFile> UploadImageAsync(IFormFile file, string containerName, string fileName)
        {
            return UploadImageAsync(file.OpenReadStream(), containerName, fileName);
        }

        public Task<BlobStorageFile> UploadImage(byte[] fileBytes, string containerName, string fileName)
        {
            return UploadImageAsync(new MemoryStream(fileBytes), containerName, fileName);
        }

        public async Task<BlobStorageFile> UploadImageAsync(Stream fileStream, string containerName, string fileName)
        {
            await using (fileStream)
            {
                BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                await blobContainerClient.CreateIfNotExistsAsync();

                BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);

                if (await blobClient.ExistsAsync())
                {
                    throw new ArgumentException(ErrorMessages.FileExists, nameof(fileName));
                }

                Response<BlobContentInfo> response = await blobClient.UploadAsync(fileStream);

                if (response.GetRawResponse().IsError)
                {
                    throw new HttpRequestException(response.GetRawResponse().ReasonPhrase);
                }

                return new BlobStorageFile(containerName, blobClient.Name, blobClient.Uri.AbsolutePath);
            }
        }

        public async Task<Stream> GetImageAsync(string containerName, string imageName)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

                if (!await containerClient.ExistsAsync())
                {
                    throw new CustomException(ErrorMessages.BlobContainerDoesntExist);
                }

                var blobClient = containerClient.GetBlobClient(imageName);

                if (!await blobClient.ExistsAsync())
                {
                    throw new CustomException(ErrorMessages.BlobDoesntExist);
                }

                var response = await blobClient.OpenReadAsync();

                return response;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
    }
}
