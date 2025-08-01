

namespace EcommerceApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime Date { get; set; }
        public User? User { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();
        public PaymentInfo? PaymentInfo { get; set; }
        public string DeliveryStatus { get; internal set; }
        public string PaymentStatus { get; internal set; }
    }
}
