using BackendProject.Models;

namespace BackendProject.Dto
{
    public class CartWithTotalDto
    {
        public List<CartItemDto>? CartItems { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
