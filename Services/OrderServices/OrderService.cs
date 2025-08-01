using Microsoft.EntityFrameworkCore;
using Razorpay.Api;
using BackendProject.Data;
using BackendProject.Dto;
using BackendProject.Models;
using BackendProject.ApiResponse;

namespace BackendProject.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public OrderService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ApiResponse<OrderViewDto>> CreateOrder_CheckOut(int userId, CreateOrderDto dto)
        {
            try
            {

                var order = new Models.Order
                {
                    UserId = userId,
                    TotalAmount = dto.TotalAmount,
                    CreatedAt = DateTime.UtcNow,
                    OrderDate = DateTime.UtcNow,
                    razorpay_order_id = dto.razorpay_order_id,
                    PaymentStatus = "Pending",
                    DeliveryStatus = "Processing" 
                };



                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                foreach (var item in dto.OrderItems)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Description = item.Description
                    };
                    await _context.OrderItems.AddAsync(orderItem);
                }

                await _context.SaveChangesAsync();

                
                var createdOrder = await _context.Orders
                    .Where(o => o.Id == order.Id)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .Select(o => new OrderViewDto
                    {
                        OrderId = o.Id,
                        TotalAmount = o.TotalAmount,
                        PaymentStatus = o.PaymentStatus,
                        DeliveryStatus = o.DeliveryStatus,
                        Items = o.OrderItems.Select(oi => new OrderItemDto
                        {
                            ProductId = oi.ProductId,
                            ProductName = oi.Product.ProductName,
                            Description = oi.Product.Description,
                            ImageUrl = oi.Product.ImageUrl,
                            Quantity = oi.Quantity
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                return ApiResponse<OrderViewDto>.SuccessResponse(createdOrder, "Order placed successfully");
            }
            catch
            {
                return ApiResponse<OrderViewDto>.FailureResponse("Checkout failed");
            }
        }


        
        public async Task<ApiResponse<List<OrderViewDto>>> GetOrderDetails(int userId)
        {
            try
            {
                var orders = await _context.Orders
                    .Where(o => o.UserId == userId)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .Select(o => new OrderViewDto
                    {
                        OrderId = o.Id,
                        TotalAmount = o.TotalAmount,
                        DeliveryStatus = o.DeliveryStatus,
                        PaymentStatus = o.PaymentStatus,
                        Items = o.OrderItems.Select(oi => new OrderItemDto
                        {
                            ProductId = oi.ProductId,
                            ProductName = oi.Product.ProductName,
                            Description = oi.Product.Description, 
                            ImageUrl = oi.Product.ImageUrl,
                            Quantity = oi.Quantity
                        }).ToList()
                    })

                    .ToListAsync();

                return ApiResponse<List<OrderViewDto>>.SuccessResponse(orders, "Orders fetched");
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"GetOrderDetails failed: {ex.Message}");
                return ApiResponse<List<OrderViewDto>>.FailureResponse("Failed to fetch orders");
            }
        }



        public async Task<ApiResponse<string>> RazorOrderCreate(long price)
        {
            try
            {
                var key = _configuration["Razorpay:Key"];
                var secret = _configuration["Razorpay:Secret"];

                RazorpayClient client = new RazorpayClient(key, secret);

                Dictionary<string, object> options = new Dictionary<string, object>
                {
                    { "amount", price * 100 },
                    { "currency", "INR" },
                    { "receipt", Guid.NewGuid().ToString() }
                };

                Razorpay.Api.Order order = client.Order.Create(options);
                var orderId = order["id"].ToString();

                return ApiResponse<string>.SuccessResponse(orderId, "Razorpay order created");
            }
            catch
            {
                return ApiResponse<string>.FailureResponse("Failed to create Razorpay order");
            }
        }

        public async Task<ApiResponse<string>> RazorPayment(PaymentDto payment)
        {
            var secret = _configuration["Razorpay:Secret"];

            Dictionary<string, string> attributes = new Dictionary<string, string>
    {
        { "razorpay_order_id", payment.razorpay_order_id },
        { "razorpay_payment_id", payment.razorpay_payment_id },
        { "razorpay_signature", payment.razorpay_signature }
    };

            try
            {
                RazorpayHelper.VerifyPaymentSignature(attributes, secret);
            }
            catch
            {
                return ApiResponse<string>.FailureResponse("Payment verification failed");
            }

            var existingOrder = await _context.Orders
                .FirstOrDefaultAsync(o => o.razorpay_order_id == payment.razorpay_order_id);

            if (existingOrder != null)
            {
                if (existingOrder.PaymentStatus != "Paid")
                {
                    existingOrder.PaymentId = payment.razorpay_payment_id;
                    existingOrder.PaymentStatus = "Paid";

                   
                    existingOrder.DeliveryStatus = "Order placed";
                }


                var saved = await _context.SaveChangesAsync() > 0;

                return saved
                    ? ApiResponse<string>.SuccessResponse("Payment verified, status updated to Paid, delivery started")
                    : ApiResponse<string>.FailureResponse("Verification succeeded but failed to update payment/delivery status");
            }


            return ApiResponse<string>.FailureResponse("Order not found");
        }

        public async Task<ApiResponse<string>> DeleteOrder(int orderId)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                    return ApiResponse<string>.FailureResponse("Order not found");

                _context.OrderItems.RemoveRange(order.OrderItems);
                _context.Orders.Remove(order);

                await _context.SaveChangesAsync();

                return ApiResponse<string>.SuccessResponse("Order deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailureResponse("Error deleting order: " + ex.Message);
            }
        }


    }
}
