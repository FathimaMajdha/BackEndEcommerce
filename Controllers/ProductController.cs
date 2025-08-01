using BackendProject.Dto;
using BackendProject.Services.ProductServices;
using BackendProject.ApiResponse;
using Microsoft.AspNetCore.Mvc;

namespace BackendProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productServices;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productServices, ILogger<ProductController> logger)
        {
            _productServices = productServices;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var result = await _productServices.GetAllProducts();
                return Ok(ApiResponse<IEnumerable<ProductWithCategoryDto>>.SuccessResponse(result, "All products fetched successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all products");
                return StatusCode(500, ApiResponse<string>.FailureResponse("An error occurred while fetching products."));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var result = await _productServices.GetProductById(id);
                if (result == null)
                    return NotFound(ApiResponse<string>.FailureResponse("Product not found"));

                return Ok(ApiResponse<ProductWithCategoryDto>.SuccessResponse(result, "Product fetched successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching product by ID {id}");
                return StatusCode(500, ApiResponse<string>.FailureResponse("An error occurred while fetching the product."));
            }
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            try
            {
                var result = await _productServices.GetProductsByCategory(categoryId);
                return Ok(ApiResponse<IEnumerable<ProductWithCategoryDto>>.SuccessResponse(result, "Products fetched successfully by category."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching products for category ID {categoryId}");
                return StatusCode(500, ApiResponse<string>.FailureResponse("An error occurred while fetching products by category."));
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProductsByFirstWord([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest(ApiResponse<string>.FailureResponse("Search keyword is required."));

            try
            {
                var results = await _productServices.SearchProductsByFirstWordAsync(keyword);
                return Ok(ApiResponse<IEnumerable<ProductWithCategoryDto>>.SuccessResponse(results, "Search results fetched successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error searching products with keyword: {keyword}");
                return StatusCode(500, ApiResponse<string>.FailureResponse("An error occurred while searching for products."));
            }
        }

    }
}
