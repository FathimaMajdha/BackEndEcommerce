using BackendProject.ApiResponse;
using BackendProject.Dto;
using BackendProject.Models;
using System.Threading.Tasks;

namespace BackendProject.Services.AdminServices
{
    public interface IAdminService
    {
        Task<ApiResponse<object>> GetDashboardOverviewAsync();
        Task<ApiResponse<List<UserOrderDto>>> GetAllUsersAsync();
        Task<ApiResponse<string>> ToggleBlockUserAsync(int userId, bool isBlocked);
        Task<ApiResponse<string>> DeleteUserAsync(int userId);

        Task<ApiResponse<List<ProductWithCategoryDto>>> GetAllProductsAsync();
        Task<ApiResponse<ProductWithCategoryDto>> GetProductByIdAsync(int id);

        Task<ApiResponse<ProductWithCategoryDto>> CreateProductAsync(CreateProductDto productDto);
        Task<ApiResponse<ProductWithCategoryDto>> UpdateProductAsync(UpdateNewProductDto dto);
        Task<ApiResponse<string>> DeleteProductAsync(int id);

        Task<ApiResponse<List<ProductWithCategoryDto>>> SearchProductsAsync(string keyword);

        Task<ApiResponse<PaginatedResponse<ProductWithCategoryDto>>> GetProductsPaginatedAsync( int page, int pageSize, string category);

        Task<ApiResponse<PaginatedResponse<UserOrderDto>>> GetUsersPaginatedAsync(int page, int pageSize, string keyword);

    }

}
