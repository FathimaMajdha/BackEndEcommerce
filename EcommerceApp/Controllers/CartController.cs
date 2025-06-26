using EcommerceApp.Data;
using EcommerceApp.Dtos;
using EcommerceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet("items")]
        public async Task<IActionResult> GetCartItems([FromQuery] int userId)
        {
            var cartItems = await _context.CartItems
                .Include(c => c.User)
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .Select(c => new CartItemDto
                {
                    Id = c.Id,
                    Quantity = c.Quantity,
                    UserId = c.UserId,
                    UserName = c.User != null ? c.User.Username : "",
                    ProductId = c.ProductId,
                    ProductTitle = c.Product != null ? c.Product.Title : "",
                    Price = c.Product != null ? c.Product.Price : 0
                })
                .ToListAsync();

            return Ok(cartItems);
        }

        
        [HttpPost]
        public async Task<ActionResult> AddToCart([FromBody] CartItemDto dto)
        {
            
            var userExists = await _context.Users.AnyAsync(u => u.Id == dto.UserId);
            var productExists = await _context.Products.AnyAsync(p => p.Id == dto.ProductId);

            if (!userExists || !productExists)
                return BadRequest("Invalid UserId or ProductId.");

            var existing = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == dto.UserId && c.ProductId == dto.ProductId);

            if (existing != null)
            {
                existing.Quantity += dto.Quantity;
            }
            else
            {
                var cartItem = new CartItem
                {
                    UserId = dto.UserId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                };
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return Ok("Item added to cart");
        }



       
        [HttpPut("{productId}")]
        public async Task<ActionResult> UpdateCartItem(int productId, [FromQuery] int userId, [FromBody] int quantity)
        {
            if (quantity <= 0)
                return BadRequest("Quantity must be greater than zero");

            var item = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == userId);

            if (item == null)
                return NotFound("Cart item not found");

            item.Quantity = quantity;
            await _context.SaveChangesAsync();

            return Ok("Cart item updated");
        }

        
        [HttpDelete("{productId}")]
        public async Task<ActionResult> DeleteCartItem(int productId, [FromQuery] int userId)
        {
            var item = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == userId);

            if (item == null)
                return NotFound("Cart item not found");

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();

            return Ok("Cart item removed");
        }
    }
}
