namespace BackendProject.Dto
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;

        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public int Quantity { get; set; }
       
    }


}
