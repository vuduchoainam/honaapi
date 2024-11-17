using honaapi.Data;
using honaapi.Interfaces;
using honaapi.Models;
using honaapi.Repositories;

namespace honaapi.Services
{
    public class UploadImageService : IUploadImageService
    {
        private readonly IRepository<UploadImage> _uploadImageRepository;
        private readonly IWebHostEnvironment _environment;

        public UploadImageService(IRepository<UploadImage> UploadImageRepository, IRepository<UploadImage> productImageRepository, IWebHostEnvironment environment)
        {
            _uploadImageRepository = UploadImageRepository;
            _environment = environment;
        }

        public async Task<List<UploadImage>> UploadImagesAsync(IEnumerable<IFormFile> images, string folderPath)
        {
            var uploadedImages = new List<UploadImage>();

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            foreach (var image in images)
            {
                if (image?.FileName != null)
                {
                    var customFileName = $"{DateTime.Now:yyyyMMddHHmmss}_{Path.GetFileNameWithoutExtension(image.FileName)}{Path.GetExtension(image.FileName)}";
                    var imagePath = Path.Combine(folderPath, customFileName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    var uploadImage = new UploadImage
                    {
                        Name = customFileName,
                        ImageUrl = "/images/product/" + customFileName
                    };

                    uploadedImages.Add(uploadImage);
                    await _uploadImageRepository.AddAsync(uploadImage);
                }
            }

            await _uploadImageRepository.SaveChangesAsync();
            return uploadedImages;
        }
    }
}
