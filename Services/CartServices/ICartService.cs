using BackendProject.ApiResponse;
using BackendProject.Dto;
using BackendProject.Models;

namespace BackendProject.Services.CartServices
{
    public interface ICartService
    {
        Task<ApiResponse<CartWithTotalDto>> GetCartByUserId(int userId);
        Task<ApiResponse<string>> AddToCart(AddToCartDto dto);
        Task<ApiResponse<string>> RemoveFromCart(int userId, int productId);
        Task<ApiResponse<string>> UpdateMultipleQuantities(int userId, List<UpdateCartQtyDto> items);


    }
}
