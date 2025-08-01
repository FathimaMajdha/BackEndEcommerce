using BackendProject.Services.WishListServices;
using Microsoft.AspNetCore.Mvc;
using BackendProject.ApiResponse;
using BackendProject.Dto;

namespace BackendProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IWishListService _wishListService;

        public WishListController(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }

        [HttpPost("{userId}/add/{productId}")]
        public async Task<IActionResult> AddToWishList(int userId, int productId)
        {
            var response = await _wishListService.AddToWishList(userId, productId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{userId}/remove/{productId}")]
        public async Task<IActionResult> RemoveFromWishList(int userId, int productId)
        {
            var response = await _wishListService.RemoveFromWishList(userId, productId);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetWishList(int userId)
        {
            var response = await _wishListService.GetWishList(userId);
            return Ok(response);
        }

       
    }
}
