namespace BackendProject.Dto
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string? OrderString { get; set; }
        public DateTime OrderDate { get; set; }
        public string? OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }

        public List<OrderItemDto> OrderItems { get; set; } = new();
    }
}
