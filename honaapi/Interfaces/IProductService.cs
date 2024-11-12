using honaapi.DTOs.product;
using honaapi.DTOs.variant;
using honaapi.Helpers;
using honaapi.Models;

namespace honaapi.Interfaces
{
    public interface IProductService
    {
        Task<PagedResult<ProductWithVariantsDTO>> SearchProductsAsync(SearchProductDTO searchDTO);
        Task<Product> GetByIdAsync(int id);
        Task AddProductWithVariantsAsync(Product product, List<CreateOrUpdateVariantDTO> variants);
        Task SaveChangesAsync();
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task<int> CountWithSlugAsync(string slug);
    }
}
