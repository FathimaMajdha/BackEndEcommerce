using EcommerceApp.Data;
using EcommerceApp.Dtos;
using EcommerceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Controllers
{
    [Route("api/wishlist")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WishlistController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetWishlistItems(int userId)
        {
            var wishlist = await _context.WishlistItems
                .Include(w => w.User)          
                .Include(w => w.Product)
                .Where(w => w.UserId == userId)
                .Select(w => new
                {
                    w.Id,
                    w.UserId,
                    Username = w.User != null ? w.User.Username : "N/A", 
                    w.ProductId,
                    Product = new
                    {
                        w.Product.Id,
                        w.Product.ImageUrl,
                        w.Product.Title,
                        w.Product.Description,
                        w.Product.Price,
                        w.Product.Reviews,
                        w.Product.Quantity,
                        w.Product.Category
                    }
                })
                .ToListAsync();

            return Ok(wishlist);
        }


        
        [HttpPost]
        public async Task<ActionResult> AddToWishlist([FromBody] WishlistItemDto dto)
        {
            if (dto == null || dto.UserId <= 0 || dto.ProductId <= 0)
                return BadRequest("Invalid request data.");

            var userExists = await _context.Users.AnyAsync(u => u.Id == dto.UserId);
            if (!userExists)
                return BadRequest($"User ID {dto.UserId} not found in Users table.");

            var productExists = await _context.Products.AnyAsync(p => p.Id == dto.ProductId);
            if (!productExists)
                return BadRequest($"Product ID {dto.ProductId} not found in Products table.");

            var alreadyExists = await _context.WishlistItems
                .AnyAsync(w => w.UserId == dto.UserId && w.ProductId == dto.ProductId);

            if (alreadyExists)
                return Conflict("Product already in wishlist.");

            var wishlistItem = new WishlistItem
            {
                UserId = dto.UserId,
                ProductId = dto.ProductId
            };

            _context.WishlistItems.Add(wishlistItem);
            await _context.SaveChangesAsync();

            return Ok("Product added to wishlist");
        }

        
        [HttpDelete("{productId}")]
        public async Task<ActionResult> RemoveFromWishlist(int productId, [FromQuery] int userId)
        {
            var item = await _context.WishlistItems
                .FirstOrDefaultAsync(w => w.ProductId == productId && w.UserId == userId);

            if (item == null)
                return NotFound("Item not found in wishlist");

            _context.WishlistItems.Remove(item);
            await _context.SaveChangesAsync();

            return Ok("Item removed from wishlist");
        }
    }
}
