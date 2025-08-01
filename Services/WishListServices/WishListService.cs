using BackendProject.Data;
using BackendProject.Models;
using BackendProject.Dto;
using BackendProject.ApiResponse;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.Services.WishListServices
{
    public class WishListService : IWishListService
    {
        private readonly AppDbContext _context;

        public WishListService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<bool>> AddToWishList(int userId, int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return ApiResponse<bool>.FailureResponse("Invalid product.");

            var wishlist = await _context.WishLists
                .Include(w => w.Items)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist == null)
            {
                wishlist = new WishList { UserId = userId };
                _context.WishLists.Add(wishlist);
                await _context.SaveChangesAsync();
            }

            bool alreadyExists = wishlist.Items.Any(i => i.ProductId == productId);
            if (alreadyExists)
                return ApiResponse<bool>.FailureResponse("Product already in wishlist.");

            wishlist.Items.Add(new WishListItem
            {
                ProductId = productId,
                Quantity = 1,
                Price = product.Price,
                WishListId = wishlist.Id
            });

            await _context.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "Product added to wishlist.");
        }

        public async Task<ApiResponse<bool>> RemoveFromWishList(int userId, int productId)
        {
            var wishlist = await _context.WishLists
                .Include(w => w.Items)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist == null)
                return ApiResponse<bool>.FailureResponse("Wishlist not found.");

            var item = wishlist.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                return ApiResponse<bool>.FailureResponse("Product not found in wishlist.");

            wishlist.Items.Remove(item);
            _context.WishListItems.Remove(item);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Product removed from wishlist.");
        }

        public async Task<ApiResponse<List<WishListItemDto>>> GetWishList(int userId)
        {
            var wishlist = await _context.WishLists
                .Include(w => w.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist == null)
                return ApiResponse<List<WishListItemDto>>.SuccessResponse(new List<WishListItemDto>(), "Empty wishlist.");

            var items = wishlist.Items.Select(i => new WishListItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product.ProductName,
                ImageUrl = i.Product.ImageUrl,
                Price = i.Price,
                Quantity = i.Quantity,
                TotalPrice = i.Price * i.Quantity
            }).ToList();

            return ApiResponse<List<WishListItemDto>>.SuccessResponse(items, "Wishlist fetched successfully.");
        }

       
    }
}
