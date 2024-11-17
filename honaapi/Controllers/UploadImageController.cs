using honaapi.Interfaces;
using honaapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace honaapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadImageController : ControllerBase
    {
        private readonly IUploadImageService _uploadImageService;
        private readonly IWebHostEnvironment _environment;
        public UploadImageController(IUploadImageService uploadImageService, IWebHostEnvironment environment)
        {
            _uploadImageService = uploadImageService;
            _environment = environment;
        }

        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImages([FromForm] List<IFormFile> images)
        {
            try
            {
                if (images == null || images.Count == 0)
                {
                    return BadRequest("No images were uploaded.");
                }

                var folderPath = Path.Combine(_environment.WebRootPath, "images", "product");
                var uploadedImages = await _uploadImageService.UploadImagesAsync(images, folderPath);

                var result = uploadedImages.Select(img => new
                {
                    img.Id,
                    img.Name,
                    img.ImageUrl
                });

                return StatusCode(201, new { message = "Images uploaded successfully", result });
            }catch(Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the product", error = ex.Message });
            }
        }

    }
}
