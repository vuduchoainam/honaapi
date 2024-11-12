using honaapi.Data;
using honaapi.DTOs.product;
using honaapi.DTOs.variant;
using honaapi.Helpers;
using honaapi.Interfaces;
using honaapi.Models;
using honaapi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace honaapi.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<VariantProduct> _variantRepository;
        private readonly IRepository<UploadImage> _uploadImageRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;

        public ProductService(IRepository<Product> productRepository, IRepository<VariantProduct> variantRepository, IRepository<UploadImage> productImageRepository, IWebHostEnvironment environment, ApplicationDbContext context)
        {
            _productRepository = productRepository;
            _variantRepository = variantRepository;
            _uploadImageRepository = productImageRepository;
            _environment = environment;
            _context = context;
        }

        public async Task<PagedResult<ProductWithVariantsDTO>> SearchProductsAsync(SearchProductDTO searchDTO)
        {
            if (searchDTO.PageSize <= 0)
            {
                searchDTO.PageSize = 10;
            }
            if (searchDTO.PageNumber < 0)
            {
                searchDTO.PageNumber = 0;
            }

            var query = _context.Products
                                .Include(p => p.VariantProducts) // Directly include VariantProducts
                                .AsQueryable();

            if (searchDTO.Id.HasValue)
            {
                query = query.Where(p => p.Id == searchDTO.Id.Value);
            }

            if (!string.IsNullOrEmpty(searchDTO.KeyWord))
            {
                query = query.Where(p => p.Name.Contains(searchDTO.KeyWord));
            }

            var totalCount = await query.CountAsync();
            var skip = searchDTO.PageNumber * searchDTO.PageSize;
            var items = await query.Skip(skip).Take(searchDTO.PageSize)
                                   .Select(p => new ProductWithVariantsDTO
                                   {
                                       Id = p.Id,
                                       Name = p.Name,
                                       Slug = p.Slug,
                                       Description = p.Description,
                                       BasePrice = p.BasePrice,
                                       CategoryId = p.CategoryId,
                                       BrandId = p.BrandId,
                                       ImageId = p.ImageId,
                                       Sold = p.Sold,
                                       CreatedAt = p.CreatedAt,
                                       UpdatedAt = p.UpdatedAt,
                                       VariantProducts = p.VariantProducts.Select(v => new VariantProductResponseDTO
                                       {
                                           Name = v.Name,
                                           Price = v.Price,
                                           InventoryQuantity = v.InventoryQuantity,
                                           Status = v.Status,
                                           CreatedAt = v.CreatedAt,
                                           UpdatedAt = v.UpdatedAt,
                                       }).ToList()
                                   }).ToListAsync();

            return new PagedResult<ProductWithVariantsDTO>
            {
                PageNumber = searchDTO.PageNumber,
                PageSize = searchDTO.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)searchDTO.PageSize),
                Items = items
            };
        }


        public async Task<Product> GetByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task AddProductWithVariantsAsync(Product product, List<CreateOrUpdateVariantDTO> variants)
        {
            // Thêm sản phẩm chính
            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            foreach(var variant in product.VariantProducts)
            {
                await _variantRepository.AddAsync(variant);
                await _variantRepository.SaveChangesAsync();
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _productRepository.UpdateAsync(product);
            await SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product != null)
            {
                await _productRepository.RemoveAsync(product);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _productRepository.SaveChangesAsync();
        }

        public async Task<int> CountWithSlugAsync(string slug)
        {
            var products = await _productRepository.FindAsync(x => x.Slug == slug);
            return products.Count();
        }
    }
}
