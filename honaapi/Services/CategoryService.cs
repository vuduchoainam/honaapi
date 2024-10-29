using honaapi.DTOs.category;
using honaapi.Helpers;
using honaapi.Interfaces;
using honaapi.Models;
using honaapi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace honaapi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<PagedResult<Category>> SearchCategoriesAsync(SearchDTO searchDTO)
        {
            if (searchDTO.PageSize <= 0)
            {
                searchDTO.PageSize = 5;
            }
            if (searchDTO.PageNumber < 0)
            {
                searchDTO.PageNumber = 0;
            }

            var query = _categoryRepository.GetQueryable();

            //get by id
            if (searchDTO.Id.HasValue)
            {
                query = query.Where(c => c.Id == searchDTO.Id.Value);
            }

            //search
            if (!string.IsNullOrEmpty(searchDTO.KeyWord))
            {
                query = query.Where(c => c.Name.Contains(searchDTO.KeyWord) ||
                                         c.Description.Contains(searchDTO.KeyWord));
            }

            //sort (ASC/DESC)
            var orderBy = !string.IsNullOrEmpty(searchDTO.OrderBy) ? searchDTO.OrderBy : "Id";

            bool desc = searchDTO.OrderByDirection.Equals("DESC", StringComparison.OrdinalIgnoreCase);
            query = query.OrderByDynamic(searchDTO.OrderBy, desc);

            //count total resource
            var totalCount = await query.CountAsync();
            var skip = searchDTO.PageNumber * searchDTO.PageSize;
            var items = await query.Skip(skip).Take(searchDTO.PageSize).ToListAsync();

            var result = new PagedResult<Category>
            {
                PageNumber = searchDTO.PageNumber,
                PageSize = searchDTO.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)searchDTO.PageSize),
                Items = items
            };
            return result;
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _categoryRepository.AddAsync(category);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _categoryRepository.UpdateAsync(category);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                await _categoryRepository.RemoveAsync(category);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task<int> CountWithSlugAsync(string slug)
        {
            var categories = await _categoryRepository.FindAsync(x => x.Slug == slug);
            return categories.Count();
        }
    }
}
