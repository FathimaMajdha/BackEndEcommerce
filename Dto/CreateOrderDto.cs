using System.ComponentModel.DataAnnotations;

namespace BackendProject.Dto
{
    public class CreateOrderDto
    {
        public long TotalAmount { get; set; }
        public string? razorpay_order_id { get; set; }
        public List<OrderItemDto>? OrderItems { get; set; }
    }

}
