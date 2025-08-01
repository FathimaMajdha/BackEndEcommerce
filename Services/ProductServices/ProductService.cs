using BackendProject.Data;
using BackendProject.Dto;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductWithCategoryDto>> GetAllProducts()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Select(p => new ProductWithCategoryDto
                {
                    Id = p.ProductId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.CategoryName
                })
                .ToListAsync();

            return products;
        }

        public async Task<ProductWithCategoryDto?> GetProductById(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.ProductId == id)
                .Select(p => new ProductWithCategoryDto
                {
                    Id = p.ProductId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.CategoryName
                })
                .FirstOrDefaultAsync();

            return product;
        }

        public async Task<IEnumerable<ProductWithCategoryDto>> GetProductsByCategory(int categoryId)
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId)
                .Select(p => new ProductWithCategoryDto
                {
                    Id = p.ProductId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.CategoryName
                })
                .ToListAsync();

            return products;
        }

        public async Task<IEnumerable<ProductWithCategoryDto>> SearchProductsByFirstWordAsync(string keyword)
        {
            var loweredKeyword = keyword.Trim().ToLower();

            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => EF.Functions.Like(
                    p.ProductName.ToLower(), $"{loweredKeyword}%"))
                .Select(p => new ProductWithCategoryDto
                {
                    Id = p.ProductId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.CategoryName
                })
                .ToListAsync();

            return products;
        }

    }
}
