using BackendProject.Dto;
using BackendProject.Services.OrderServices;
using BackendProject.ApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendProject2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetOrders(int userId)
        {
            var response = await _orderService.GetOrderDetails(userId);
            return response.Success ? Ok(response) : StatusCode(500, response);
        }

        [HttpPost("razorpay/create")]
        public async Task<IActionResult> RazorCreateOrder([FromBody] RazorCreateDto model)
        {
            if (model == null || model.Price <= 0)
            {
                return BadRequest(ApiResponse<string>.FailureResponse("Invalid price"));
            }

            var response = await _orderService.RazorOrderCreate(model.Price);
            return response.Success ? Ok(response) : StatusCode(500, response);
        }

        [HttpPost("razorpay/verify")]
        public async Task<IActionResult> VerifyPayment([FromBody] PaymentDto payment)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(ApiResponse<string>.FailureResponse(
                    "Invalid payment data: " + string.Join(", ", errors)));
            }

            var response = await _orderService.RazorPayment(payment);
            return response.Success ? Ok(response) : BadRequest(response);
        }




        [HttpPost("checkout/{userId}")]
        public async Task<IActionResult> CheckoutOrder(int userId, [FromBody] CreateOrderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.FailureResponse("Invalid checkout data"));

            var response = await _orderService.CreateOrder_CheckOut(userId, dto);
            return response.Success ? Ok(response) : StatusCode(500, response);
        }


        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var response = await _orderService.DeleteOrder(orderId);
            return response.Success ? Ok(response) : StatusCode(500, response);
        }


        
    }
}
