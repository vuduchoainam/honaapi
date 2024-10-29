using honaapi.DTOs.category;
using honaapi.Helpers;
using honaapi.Models;

namespace honaapi.Interfaces
{
    public interface ICategoryService
    {
        Task<PagedResult<Category>> SearchCategoriesAsync(SearchCategoryDTO searchDTO);
        Task<Category> GetByIdAsync(int id);
        Task AddCategoryAsync(Category category);
        Task SaveChangesAsync();
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
        Task<int> CountWithSlugAsync(string slug);
    }
}
