using BackendProject.ApiResponse;
using BackendProject.Dto;

namespace BackendProject.Services.OrderServices
{
    public interface IOrderService
    {
        Task<ApiResponse<OrderViewDto>> CreateOrder_CheckOut(int userId, CreateOrderDto dto);
        Task<ApiResponse<List<OrderViewDto>>> GetOrderDetails(int userId);
        Task<ApiResponse<string>> RazorOrderCreate(long Price);
        Task<ApiResponse<string>> RazorPayment(PaymentDto payment);

        Task<ApiResponse<string>> DeleteOrder(int orderId);

    }


}



