using EcommerceApp.Data;
using EcommerceApp.Dtos;
using EcommerceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetOrders(int userId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.PaymentInfo)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.Date)
                .ToListAsync();

            var response = orders.Select(o => new OrderResponseDto
            {
                Id = o.Id,
                Date = o.Date,
                TotalAmount = o.TotalAmount,
                PaymentInfo = new PaymentInfoDto
                {
                    CardholderName = o.PaymentInfo.CardholderName,
                    CardNumber = o.PaymentInfo.CardNumber,
                    ExpirationDate = o.PaymentInfo.ExpirationDate,
                    CVV = o.PaymentInfo.CVV
                },
                OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductTitle = oi.Product?.Title ?? "N/A",
                    Price = oi.Product?.Price ?? 0,
                    Quantity = oi.Quantity
                }).ToList()
            }).ToList();

            return Ok(response);
        }



        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderRequestDto dto)
        {
            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == dto.UserId)
                .ToListAsync();

            if (!cartItems.Any())
                return BadRequest("Cart is empty");

            if (dto.PaymentInfo == null)
                return BadRequest("Payment information is required");

            var totalAmount = cartItems
                .Where(i => i.Product != null)
                .Sum(i => i.Product!.Price * i.Quantity);

            var order = new Order
            {
                UserId = dto.UserId,
                Date = DateTime.UtcNow,
                TotalAmount = totalAmount,
                OrderItems = cartItems.Select(c => new OrderItem
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity
                }).ToList(),
                PaymentInfo = new PaymentInfo
                {
                    CardholderName = dto.PaymentInfo.CardholderName ?? string.Empty,
                    CardNumber = dto.PaymentInfo.CardNumber ?? string.Empty,
                    ExpirationDate = dto.PaymentInfo.ExpirationDate ?? string.Empty,
                    CVV = dto.PaymentInfo.CVV ?? string.Empty
                }
            };

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Order placed successfully", OrderId = order.Id });
        }
    }
}
