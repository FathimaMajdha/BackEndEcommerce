namespace BackendProject.Dto
{
    public class CartUpdateDto
    {
        public int UserId { get; set; }
        public ICollection<CartItemDto>? Items { get; set; }
    }

   
}
