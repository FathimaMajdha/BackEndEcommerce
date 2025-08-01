using BackendProject.ApiResponse;
using BackendProject.Dto;
using BackendProject.Services.CartServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCartByUserId(int userId)
        {
            var result = await _cartService.GetCartByUserId(userId);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(AddToCartDto dto)
        {
            var result = await _cartService.AddToCart(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{userId}/remove/{productId}")]
        public async Task<IActionResult> RemoveFromCart(int userId, int productId)
        {
            var result = await _cartService.RemoveFromCart(userId, productId);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{userId}/update-qty")]
        public async Task<IActionResult> UpdateCartItems(int userId, [FromBody] UpdateCartItemsDto dto)
        {
            var result = await _cartService.UpdateMultipleQuantities(userId, dto.Items);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

    }
}
