using Microsoft.AspNetCore.Http;

namespace TFSport.Models.DTOModels.Images
{
    public record UploadDocumentVersionRequest
    {
        public IFormFile File { get; init; }
    }
}
