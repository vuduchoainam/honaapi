using honaapi.DTOs.category;
using honaapi.Helpers;
using honaapi.Interfaces;
using honaapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace honaapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("SearchCategory")]
        public async Task<IActionResult> Search([FromBody] SearchCategoryDTO searchDTO)
        {
            try
            {
                var pagedResult = await _categoryService.SearchCategoriesAsync(searchDTO);

                var items = pagedResult.Items.Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Description,
                    c.Slug,
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

        [HttpPost("CreateCategory")]
        public async Task<IActionResult> Create([FromBody] CreateOrUpdateCategoryDTO createDTO)
        {
            if (string.IsNullOrEmpty(createDTO.Name))
            {
                return StatusCodeResponse.BadRequestResponse("Category name cannot be empty", "Category name cannot be empty");
            }
            try
            {
                var category = new Category
                {
                    Name = createDTO.Name,
                    Description = createDTO.Description,
                    Slug = StringUtil.GenerateSlug(createDTO.Name),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                var originalSlug = category.Slug;
                var slugIndex = 1;
                while (await _categoryService.CountWithSlugAsync(category.Slug) > 0)
                {
                    // Nếu slug đã tồn tại
                    category.Slug = $"{originalSlug}-{slugIndex}";
                    slugIndex++;
                }

                await _categoryService.AddCategoryAsync(category);
                await _categoryService.SaveChangesAsync();

                var result = new
                {
                    category.Id,
                    category.Name,
                    category.Slug,
                    category.Description,
                    CreatedAt = StringUtil.FormatDate(category.CreatedAt),
                    UpdatedAt = StringUtil.FormatDate(category.UpdatedAt)
                };

                return StatusCodeResponse.CreatedResponse(result);
            }
            catch (Exception ex)
            {
                return StatusCodeResponse.InternalServerErrorResponse("An error occurred while processing the request", ex.Message);
            }
        }

        [HttpPut("EditCategory/{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] CreateOrUpdateCategoryDTO editDTO)
        {
            if (string.IsNullOrEmpty(editDTO.Name))
            {
                return StatusCodeResponse.BadRequestResponse("Category name cannot be empty", "Category name cannot be empty");
            }
            try
            {
                var category = await _categoryService.GetByIdAsync(id);
                if (category == null)
                {
                    return StatusCodeResponse.NotFoundResponse("Category not found", "Category not found");
                }

                category.Name = editDTO.Name;
                category.Description = editDTO.Description;
                category.Slug = StringUtil.GenerateSlug(editDTO.Name);
                category.UpdatedAt = DateTime.Now;

                await _categoryService.UpdateCategoryAsync(category);
                await _categoryService.SaveChangesAsync();

                var result = new
                {
                    category.Id,
                    category.Name,
                    category.Slug,
                    category.Description,
                    CreatedAt = StringUtil.FormatDate(category.CreatedAt),
                    UpdatedAt = StringUtil.FormatDate(category.UpdatedAt)
                };

                return StatusCodeResponse.SuccessResponse(result, "Edit Category successfully");
            }
            catch (Exception ex)
            {
                return StatusCodeResponse.InternalServerErrorResponse("An error occurred while processing the request", ex.Message);
            }
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _categoryService.GetByIdAsync(id);
                if (category == null)
                {
                    return StatusCodeResponse.NotFoundResponse("Category not found", "Category not found");
                }

                await _categoryService.DeleteCategoryAsync(id);
                await _categoryService.SaveChangesAsync();

                var result = new
                {
                    category.Id
                };

                return StatusCodeResponse.SuccessResponse("Delete Category successfully");
            }
            catch (Exception ex)
            {
                return StatusCodeResponse.InternalServerErrorResponse("An error occurred while processing the request", ex.Message);
            }
        }
    }
}
