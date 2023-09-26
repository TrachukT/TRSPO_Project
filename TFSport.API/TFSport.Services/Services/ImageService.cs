using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TFSport.Models;
using TFSport.Models.Exceptions;
using TFSport.Services.Interfaces;

namespace TFSport.Services.Services
{
    public class ImageService : IImageService
    {
        private readonly IBlobStorageService _blobStorageService;
        private readonly BlobStorageOptions _blobOptions;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImageService(IOptions<BlobStorageOptions> blobOptions, IBlobStorageService blobStorageService, ILogger<ArticleService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _blobStorageService = blobStorageService;
            _blobOptions = blobOptions.Value;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                {
                    throw new CustomException(ErrorMessages.NoImageProvided);
                }

                if (imageFile.Length > 2 * 1024 * 1024)
                {
                    throw new CustomException(ErrorMessages.ImageSizeExceeded);
                }

                var allowedFormats = new[] { "image/jpeg", "image/png" };
                if (!allowedFormats.Contains(imageFile.ContentType))
                {
                    throw new CustomException(ErrorMessages.InvalidImageFormat);
                }

                var imageName = Path.GetFileName(imageFile.FileName);

                var imageStream = imageFile.OpenReadStream();
                var blobStorageFile = await _blobStorageService.UploadImageAsync(imageStream, _blobOptions.ImageContainer, imageName);

                var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
                var downloadUrl = $"{baseUrl}/images/{blobStorageFile.FileName}";

                _logger.LogInformation("File {FileName} was uploaded to Blob Storage.", blobStorageFile.FileName);

                return downloadUrl;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<Stream> GetImageAsync(string imageName)
        {
            try
            {
                string containerName = _blobOptions.ImageContainer;

                return await _blobStorageService.GetImageAsync(containerName, imageName);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
    }
}
