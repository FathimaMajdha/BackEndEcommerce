namespace EcommerceApp.Dtos
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new();
        public PaymentInfoDto PaymentInfo { get; set; } = new();
    }

}
