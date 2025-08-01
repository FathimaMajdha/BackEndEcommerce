using BackendProject.Data;
using BackendProject.Dto;
using BackendProject.Models;
using BackendProject.ApiResponse;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<IEnumerable<CategoryDto>>> GetAllCategories()
        {
            try
            {
                var categories = await _context.Categories
                    .Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        CategoryName = c.CategoryName
                    }).ToListAsync();

                return ApiResponse<IEnumerable<CategoryDto>>.SuccessResponse(categories, "Fetched categories successfully");
            }
            catch
            {
                return ApiResponse<IEnumerable<CategoryDto>>.FailureResponse("Failed to fetch categories");
            }
        }

        public async Task<ApiResponse<CategoryDto>> GetCategoryById(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                    return ApiResponse<CategoryDto>.FailureResponse("Category not found");

                var dto = new CategoryDto
                {
                    Id = category.Id,
                    CategoryName = category.CategoryName
                };

                return ApiResponse<CategoryDto>.SuccessResponse(dto, "Category retrieved");
            }
            catch
            {
                return ApiResponse<CategoryDto>.FailureResponse("Error retrieving category");
            }
        }

        public async Task<ApiResponse<string>> CreateCategory(CatAddDto dto)
        {
            try
            {
                var category = new Category
                {
                    CategoryName = dto.CategoryName
                };

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return ApiResponse<string>.SuccessResponse("Category created successfully");
            }
            catch
            {
                return ApiResponse<string>.FailureResponse("Failed to create category");
            }
        }

        public async Task<ApiResponse<string>> UpdateCategory(int id, UpdateCategoryDto dto)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                    return ApiResponse<string>.FailureResponse("Category not found");

                category.CategoryName = dto.CategoryName;

                _context.Categories.Update(category);
                await _context.SaveChangesAsync();

                return ApiResponse<string>.SuccessResponse("Category updated successfully");
            }
            catch
            {
                return ApiResponse<string>.FailureResponse("Failed to update category");
            }
        }

        public async Task<ApiResponse<string>> DeleteCategory(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                    return ApiResponse<string>.FailureResponse("Category not found");

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return ApiResponse<string>.SuccessResponse("Category deleted successfully");
            }
            catch
            {
                return ApiResponse<string>.FailureResponse("Failed to delete category");
            }
        }
    }
}
