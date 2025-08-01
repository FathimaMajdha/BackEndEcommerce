namespace BackendProject.Dto
{
    public class OrderSummaryDto
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CartItemSummaryDto> CartItems { get; set; } = new();
    }
}
