using honaapi.DTOs.brand;
using honaapi.Helpers;
using honaapi.Interfaces;
using honaapi.Models;
using honaapi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace honaapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpPost("SearchBrand")]
        public async Task<IActionResult> Search([FromBody] SearchBrandDTO searchDTO)
        {
            try
            {
                var pagedResult = await _brandService.SearchBrandAsync(searchDTO);

                var items = pagedResult.Items.Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Description,
                    CreatedAt = StringUtil.FormatDate(c.CreatedAt),
                    UpdatedAt = StringUtil.FormatDate(c.UpdatedAt)
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

        [HttpPost("CreateBrand")]
        public async Task<IActionResult> Create([FromBody] CreateOrUpdateBrandDTO createDTO)
        {
            if (string.IsNullOrEmpty(createDTO.Name))
            {
                return StatusCodeResponse.BadRequestResponse("Brand name cannot be empty", "Brand name cannot be empty");
            }
            try
            {
                var brand = new Brand
                {
                    Name = createDTO.Name,
                    Description = createDTO.Description,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                await _brandService.AddBrandAsync(brand);
                await _brandService.SaveChangesAsync();

                var result = new
                {
                    brand.Id,
                    brand.Name,
                    brand.Description,
                    CreatedAt = StringUtil.FormatDate(brand.CreatedAt),
                    UpdatedAt = StringUtil.FormatDate(brand.UpdatedAt)
                };

                return StatusCodeResponse.CreatedResponse(result);
            }
            catch (Exception ex)
            {
                return StatusCodeResponse.InternalServerErrorResponse("An error occurred while processing the request", ex.Message);
            }
        }

        [HttpPut("EditBrand/{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] CreateOrUpdateBrandDTO editDTO)
        {
            if (string.IsNullOrEmpty(editDTO.Name))
            {
                return StatusCodeResponse.BadRequestResponse("Brand name cannot be empty", "Brand name cannot be empty");
            }
            try
            {
                var brand = await _brandService.GetByIdAsync(id);
                if (brand == null)
                {
                    return StatusCodeResponse.NotFoundResponse("Brand not found", "Brand not found");
                }

                brand.Name = editDTO.Name;
                brand.Description = editDTO.Description;
                brand.UpdatedAt = DateTime.Now;

                await _brandService.UpdateBrandAsync(brand);
                await _brandService.SaveChangesAsync();

                var result = new
                {
                    brand.Id,
                    brand.Name,
                    brand.Description,
                    CreatedAt = StringUtil.FormatDate(brand.CreatedAt),
                    UpdatedAt = StringUtil.FormatDate(brand.UpdatedAt)
                };

                return StatusCodeResponse.SuccessResponse(result, "Edit Brand successfully");
            }
            catch (Exception ex)
            {
                return StatusCodeResponse.InternalServerErrorResponse("An error occurred while processing the request", ex.Message);
            }
        }

        [HttpDelete("DeleteBrand/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var brand = await _brandService.GetByIdAsync(id);
                if (brand == null)
                {
                    return StatusCodeResponse.NotFoundResponse("Brand not found", "Brand not found");
                }

                await _brandService.DeleteBrandAsync(id);
                await _brandService.SaveChangesAsync();

                var result = new
                {
                    brand.Id
                };

                return StatusCodeResponse.SuccessResponse("Delete Brand successfully");
            }
            catch (Exception ex)
            {
                return StatusCodeResponse.InternalServerErrorResponse("An error occurred while processing the request", ex.Message);
            }
        }
    }
}
