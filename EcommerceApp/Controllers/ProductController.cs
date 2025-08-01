using EcommerceApp.Data;
using EcommerceApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Controllers
{

    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound("Product not found");
            return product;
        }

        
        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(int category)
        {
            var products = await _context.Products
                .Where(p => p.Category== category)
                .ToListAsync();

            if (!products.Any()) return NotFound("No products found for this category");

            return products;
        }

        
        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<Product>>> GetPaginatedProducts(int page = 1, int pageSize = 5)
        {
            var products = await _context.Products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return products;
        }

        
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProducts(string query)
        {
            var products = await _context.Products
                .Where(p => p.Title.Contains(query) || p.Description.Contains(query))
                .ToListAsync();

            if (!products.Any()) return NotFound("No matching products found");

            return products;
        }
    }
}