using Microsoft.AspNetCore.Http;
namespace TFSport.Services.Interfaces
{
    public interface IImageService
    {
        public Task<string> UploadImageAsync(IFormFile imageFile);

        public Task<Stream> GetImageAsync(string imageName);
    }
}