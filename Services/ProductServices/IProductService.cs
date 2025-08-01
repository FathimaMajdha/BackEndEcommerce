using BackendProject.Dto;

namespace BackendProject.Services.ProductServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductWithCategoryDto>> GetAllProducts();
        Task<ProductWithCategoryDto?> GetProductById(int id);
        Task<IEnumerable<ProductWithCategoryDto>> GetProductsByCategory(int categoryId);

        Task<IEnumerable<ProductWithCategoryDto>> SearchProductsByFirstWordAsync(string keyword);

    }
}
