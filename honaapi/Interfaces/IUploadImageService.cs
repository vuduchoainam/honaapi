using honaapi.Models;
using honaapi.Repositories;
using honaapi.Services;

namespace honaapi.Interfaces
{
    public interface IUploadImageService
    {
        Task<List<UploadImage>> UploadImagesAsync(IEnumerable<IFormFile> images, string folderPath);
    }
}
