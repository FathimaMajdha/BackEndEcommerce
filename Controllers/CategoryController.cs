using BackendProject.Dto;
using Microsoft.AspNetCore.Mvc;
using BackendProject.ApiResponse;
using Microsoft.AspNetCore.Authorization;
using BackendProject.Services.CategoryServices;

namespace BackendProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryServices;

        public CategoryController(ICategoryService categoryServices)
        {
            _categoryServices = categoryServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var response = await _categoryServices.GetAllCategories();
            return response.Success ? Ok(response) : StatusCode(500, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var response = await _categoryServices.GetCategoryById(id);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory([FromBody] CatAddDto categoryDto)
        {
            var response = await _categoryServices.CreateCategory(categoryDto);
            return response.Success ? Ok(response) : StatusCode(500, response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto categoryDto)
        {
            var response = await _categoryServices.UpdateCategory(id, categoryDto);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var response = await _categoryServices.DeleteCategory(id);
            return response.Success ? Ok(response) : NotFound(response);
        }
    }
}
