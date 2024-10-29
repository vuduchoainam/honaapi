using honaapi.DTOs.brand;
using honaapi.Helpers;
using honaapi.Interfaces;
using honaapi.Models;
using honaapi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace honaapi.Services
{
    public class BrandService : IBrandService
    {
        private readonly IRepository<Brand> _brandRepository;
        public BrandService(IRepository<Brand> brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<PagedResult<Brand>> SearchBrandAsync(SearchBrandDTO searchDTO)
        {
            if (searchDTO.PageSize <= 0)
            {
                searchDTO.PageSize = 5;
            }
            if (searchDTO.PageNumber < 0)
            {
                searchDTO.PageNumber = 0;
            }

            var query = _brandRepository.GetQueryable();

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

            var result = new PagedResult<Brand>
            {
                PageNumber = searchDTO.PageNumber,
                PageSize = searchDTO.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)searchDTO.PageSize),
                Items = items
            };
            return result;
        }

        public async Task<Brand> GetByIdAsync(int id)
        {
            return await _brandRepository.GetByIdAsync(id);
        }

        public async Task AddBrandAsync(Brand brand)
        {
            await _brandRepository.AddAsync(brand);
        }

        public async Task UpdateBrandAsync(Brand brand)
        {
            _brandRepository.UpdateAsync(brand);
            await _brandRepository.SaveChangesAsync();

        }

        public async Task DeleteBrandAsync(int id)
        {
            var category = await _brandRepository.GetByIdAsync(id);
            if (category != null)
            {
                await _brandRepository.RemoveAsync(category);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _brandRepository.SaveChangesAsync();
        }
    }
}
