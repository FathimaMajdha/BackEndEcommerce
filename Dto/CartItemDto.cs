
namespace BackendProject.Dto
{
    public class CartItemDto
    {
        public int CartId { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }

        public ProductDto? Product { get; set; }
    }
}