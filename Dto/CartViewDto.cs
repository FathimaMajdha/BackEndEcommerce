namespace BackendProject.Dto
{
    public class CartViewDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public string? ProductImage { get; internal set; }
        public decimal ProductPrice { get; internal set; }
    }
}
