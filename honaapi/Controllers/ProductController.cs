using honaapi.DTOs.brand;
using honaapi.DTOs.product;
using honaapi.DTOs.variant;
using honaapi.Helpers;
using honaapi.Interfaces;
using honaapi.Models;
using honaapi.Repositories;
using honaapi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace honaapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> Create([FromBody] CreateOrUpdateProductDTO createDTO)
        {
            if (string.IsNullOrEmpty(createDTO.Name))
            {
                return StatusCodeResponse.BadRequestResponse("Product name cannot be empty", "Product name cannot be empty");
            }
            try
            {
                var product = new Product
                {
                    Name = createDTO.Name,
                    Slug = StringUtil.GenerateSlug(createDTO.Name),
                    Description = createDTO.Description,
                    BasePrice = createDTO.BasePrice,
                    BrandId = createDTO.BrandId,
                    CategoryId = createDTO.CategoryId,
                    Sold = createDTO.Sold,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                //slug
                var originalSlug = product.Slug;
                var slugIndex = 1;
                while (await _productService.CountWithSlugAsync(product.Slug) > 0)
                {
                    product.Slug = $"{originalSlug}-{slugIndex}";
                    slugIndex++;
                }

                foreach (var variantDTO in createDTO.Variants)
                {
                    var variant = new VariantProduct
                    {
                        ProductId = product.Id,
                        Name = variantDTO.Name,
                        Price = variantDTO.Price,
                        InventoryQuantity = variantDTO.InventoryQuantity,
                        Status = variantDTO.Status,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    };

                    product.VariantProducts.Add(variant);
                }
                // Thêm sản phẩm và các biến thể liên quan
                await _productService.AddProductWithVariantsAsync(product, createDTO.Variants);

                    return StatusCode(201, new { message = "Product created successfully", product });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the product", error = ex.Message });
            }
        }

        [HttpPost("SearchProduct")]
        public async Task<IActionResult> Search([FromBody] SearchProductDTO searchDTO)
        {
            try
            {
                var pagedResult = await _productService.SearchProductsAsync(searchDTO);

                var items = pagedResult.Items.Select(p => new
                {
                    p.Id,
                    p.Name, 
                    p.Slug,
                    p.Description,
                    p.BasePrice,
                    p.CategoryId,
                    p.BrandId,
                    p.ImageId,
                    p.Sold,
                    CreatedAt = StringUtil.FormatDate(p.CreatedAt),
                    UpdatedAt = StringUtil.FormatDate(p.UpdatedAt),
                    VariantProducts = p.VariantProducts.Select(v => new CreateOrUpdateVariantDTO
                    {
                        Name = v.Name,
                        Price = v.Price,
                        InventoryQuantity = v.InventoryQuantity,
                        Status = v.Status
                    }).ToList()
                }).ToList();

                var result = new
                {
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount,
                    pagedResult.TotalPages,
                    Items = items
                };

                return StatusCodeResponse.SuccessResponse(result, "The request has been fulfilled, resulting in the creation of a new resource");
            }
            catch (Exception ex)
            {
                return StatusCodeResponse.InternalServerErrorResponse("An error occurred while processing the request", ex.Message);
            }
        }
    }
}
