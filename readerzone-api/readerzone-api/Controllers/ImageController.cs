using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using readerzone_api.Services.ImageService;

namespace readerzone_api.Controllers
{
    [Route("api/image")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ImageService _imageService;

        public ImageController(ImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("upload")]
        [Produces("application/json")]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            using var memoryStream = new MemoryStream();
            await imageFile.CopyToAsync(memoryStream);
            var imageData = memoryStream.ToArray();

            var imageUrl = await _imageService.UploadImage(imageData);

            return Ok(new { Url = imageUrl });
        }
    }
}
