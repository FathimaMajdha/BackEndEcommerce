namespace EcommerceApp.Dtos
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; } = "";

        public int ProductId { get; set; }
        public string ProductTitle { get; set; } = "";
        public decimal Price { get; set; }
    }
}
