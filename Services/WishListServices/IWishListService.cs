using BackendProject.ApiResponse;
using BackendProject.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BackendProject.Services.WishListServices
{
    public interface IWishListService
    {
        Task<ApiResponse<bool>> AddToWishList(int userId, int productId);
        Task<ApiResponse<bool>> RemoveFromWishList(int userId, int productId);
        Task<ApiResponse<List<WishListItemDto>>> GetWishList(int userId);
        
    }
}
