namespace BackendProject.Dto
{
    public class OrderResponseDto
    {
        public int OrderId { get; set; }
        public string? OrderString { get; set; }
        public DateTime OrderDate { get; set; }
        public string? OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }

   
        public string? RazorpayOrderId { get; set; }

        public List<OrderItemDto> OrderItems { get; set; } = new();
    }
}
