using honaapi.DTOs.brand;
using honaapi.Helpers;
using honaapi.Models;

namespace honaapi.Interfaces
{
    public interface IBrandService
    {
        Task<PagedResult<Brand>> SearchBrandAsync(SearchBrandDTO searchDTO);
        Task<Brand> GetByIdAsync(int id);
        Task AddBrandAsync(Brand brand);
        Task SaveChangesAsync();
        Task UpdateBrandAsync(Brand brand);
        Task DeleteBrandAsync(int id);
    }
}
