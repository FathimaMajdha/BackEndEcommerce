using BackendProject.ApiResponse;
using BackendProject.Dto;

namespace BackendProject.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<ApiResponse<IEnumerable<CategoryDto>>> GetAllCategories();
        Task<ApiResponse<CategoryDto>> GetCategoryById(int id);
        Task<ApiResponse<string>> CreateCategory(CatAddDto dto);
        Task<ApiResponse<string>> UpdateCategory(int id, UpdateCategoryDto dto);

        Task<ApiResponse<string>> DeleteCategory(int id);
    }
}
