using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TFSport.API.Filters;
using TFSport.Models.DTOModels.Images;
using TFSport.Models.Entities;
using TFSport.Services.Interfaces;

namespace TFSport.API.Controllers
{
    [Route("images")]
    [ApiController]
    [CustomExceptionFilter]
    [SwaggerResponse(400, "Bad_Request", typeof(string))]
    [SwaggerResponse(500, "Internal_Server_Error", typeof(string))]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        /// <summary>
        /// Uploads an image file.
        /// </summary>
        /// <remarks>
        /// Sample request for uploading an image:
        /// <code>
        /// {
        ///     "File": "Image File Data"
        /// }
        /// </code>
        /// </remarks>
        /// <param name="request">The request object containing the image file to upload.</param>
        /// <returns>The URL of the uploaded image.</returns>
        [HttpPost]
        [RoleAuthorization(UserRoles.Author, UserRoles.SuperAdmin)]
        [SwaggerResponse(200, "Request_Succeeded")]
        public async Task<ActionResult<string>> UploadImage([FromForm] UploadDocumentVersionRequest request)
        {
            var imageUrl = await _imageService.UploadImageAsync(request.File);
            return Ok(imageUrl);
        }

        /// <summary>
        /// Gets an image by its name.
        /// </summary>
        /// <param name="imageName">The name of the image to retrieve.</param>
        /// <returns>The image data.</returns>
        [HttpGet("{imageName}")]
        [SwaggerResponse(200, "Request_Succeeded")]
        public async Task<ActionResult<string>> GetImage(string imageName)
        {
            var imageUrl = await _imageService.GetImageAsync(imageName);
            return Ok(imageUrl);
        }
    }
}