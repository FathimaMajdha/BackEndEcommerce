namespace BackendProject.Dto
{
    public class CartItemSummaryDto
    {
        public string Title { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
