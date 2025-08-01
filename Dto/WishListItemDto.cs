namespace BackendProject.Dto
{
    public class WishListItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }      
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }  
    }

}
