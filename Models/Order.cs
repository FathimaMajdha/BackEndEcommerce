namespace BackendProject.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? razorpay_order_id { get; set; }
        public string? PaymentStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }

     
        public string? StreetName { get; set; }
        public string? City { get; set; }
        public string? HomeAddress { get; set; }
        public string? CustomerPhone { get; set; }
        public string? PostalCode { get; set; }

        public  List<OrderItem> OrderItems { get; set; }
        public string? DeliveryStatus { get; internal set; }
        public DateTime CreatedAt { get; internal set; }
        public string? PaymentId { get; internal set; }
        public List<CartItem>? CartItems { get; internal set; }
    }

}
