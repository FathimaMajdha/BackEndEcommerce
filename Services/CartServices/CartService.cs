using BackendProject.ApiResponse;
using BackendProject.Data;
using BackendProject.Dto;
using BackendProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.Services.CartServices
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<CartWithTotalDto>> GetCartByUserId(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                var emptyCart = new CartWithTotalDto
                {
                    CartItems = new List<CartItemDto>(),
                    TotalPrice = 0
                };

                return ApiResponse<CartWithTotalDto>.SuccessResponse(emptyCart, "Cart is empty");
            }

            var cartItemsDto = cart.CartItems.Select(ci => new CartItemDto
            {
                CartId = ci.CartId,
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                Product = new ProductDto
                {
                    ProductName = ci.Product?.ProductName ?? string.Empty,
                    Description = ci.Product?.Description ?? string.Empty,
                    Price = ci.Product?.Price ?? 0,
                    Reviews = ci.Product?.Reviews?? 0,
                    ImageUrl = ci.Product?.ImageUrl ?? string.Empty,
                    CategoryId = ci.Product?.CategoryId ?? 0
                }
            }).ToList();

            var totalPrice = cartItemsDto.Sum(i => i.Product.Price * i.Quantity);

            var response = new CartWithTotalDto
            {
                CartItems = cartItemsDto,
                TotalPrice = totalPrice
            };

            return ApiResponse<CartWithTotalDto>.SuccessResponse(response, "Cart retrieved successfully");
        }

        public async Task<ApiResponse<string>> AddToCart(AddToCartDto dto)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == dto.UserId);

            if (cart == null)
            {
                cart = new Cart { UserId = dto.UserId, CartItems = new List<CartItem>() };
                _context.Carts.Add(cart);
            }

            var existingItem = cart.CartItems.FirstOrDefault(i => i.ProductId == dto.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                });
            }

            await _context.SaveChangesAsync();
            return ApiResponse<string>.SuccessResponse("Item added to cart successfully", "Item added");
        }

        public async Task<ApiResponse<string>> RemoveFromCart(int userId, int productId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return ApiResponse<string>.FailureResponse("Cart not found");

            var item = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                return ApiResponse<string>.FailureResponse("Item not found in cart");

            cart.CartItems.Remove(item);
            await _context.SaveChangesAsync();

            return ApiResponse<string>.SuccessResponse("Item removed from cart successfully", "Item removed");
        }

        public async Task<ApiResponse<string>> UpdateMultipleQuantities(int userId, List<UpdateCartQtyDto> items)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return ApiResponse<string>.FailureResponse("Cart not found");

            foreach (var itemDto in items)
            {
                var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == itemDto.ProductId);
                if (existingItem != null)
                {
                    existingItem.Quantity = itemDto.Quantity;
                }
            }

            await _context.SaveChangesAsync();
            return ApiResponse<string>.SuccessResponse("Cart updated successfully");
        }

    }
}
