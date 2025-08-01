namespace BackendProject.Dto
{
    public class WishListViewDto
    {
        
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; internal set; }
        public string? Description { get; internal set; }
    }
}
