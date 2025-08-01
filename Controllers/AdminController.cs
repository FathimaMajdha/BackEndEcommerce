using Microsoft.AspNetCore.Mvc;
using BackendProject.Services.AdminServices;
using BackendProject.Dto;
using BackendProject.ApiResponse;
using BackendProject.Models;
using Microsoft.AspNetCore.Authorization;

namespace BackendProject.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("overview")]
       
        public async Task<IActionResult> GetOverview()
        {
            var response = await _adminService.GetDashboardOverviewAsync();
            return response.Success
                ? Ok(response)
                : BadRequest(response);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _adminService.GetAllUsersAsync();
            return response.Success
                ? Ok(response)
                : BadRequest(response);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> ToggleUserBlock(int id, [FromBody] BlockRequest model)
        {
            var response = await _adminService.ToggleBlockUserAsync(id, model.IsBlocked);
            return response.Success
                ? Ok(response)
                : NotFound(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var response = await _adminService.DeleteUserAsync(id);
            return response.Success
                ? Ok(response)
                : NotFound(response);
        }


        
        [HttpGet("products")]
        public async Task<IActionResult> GetProducts()
        {
            var response = await _adminService.GetAllProductsAsync();
            return response.Success
                ? Ok(response)
                : BadRequest(response);
        }

        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var response = await _adminService.GetProductByIdAsync(id);
            return response.Success
                ? Ok(response)
                : NotFound(response);
        }

        [HttpPost("products")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto productDto)
        {
            var response = await _adminService.CreateProductAsync(productDto);
            return response.Success
                ? Ok(response)
                : BadRequest(response);
        }

        [HttpPut("products/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] UpdateNewProductDto dto)
        {
            if (id != dto.Id)
                return BadRequest(ApiResponse<string>.FailureResponse("Product ID mismatch"));

            var response = await _adminService.UpdateProductAsync(dto);
            return response.Success
                ? Ok(response)
                : NotFound(response);
        }

        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var response = await _adminService.DeleteProductAsync(id);
            return response.Success
                ? Ok(response)
                : NotFound(response);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string keyword)
        {
            var result = await _adminService.SearchProductsAsync(keyword);
            return Ok(result);
        }

        [HttpGet("products/paginated")]
        public async Task<IActionResult> GetProductsPaginated( [FromQuery] int page = 1, [FromQuery] int pageSize = 5,  [FromQuery] string? category = "")
        {
            var response = await _adminService.GetProductsPaginatedAsync(page, pageSize, category);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("users/paginated")]
        public async Task<IActionResult> GetUsersPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 5, [FromQuery] string keyword = "")
        {
            var response = await _adminService.GetUsersPaginatedAsync(page, pageSize, keyword);
            return response.Success ? Ok(response) : BadRequest(response);
        }





    }
}
