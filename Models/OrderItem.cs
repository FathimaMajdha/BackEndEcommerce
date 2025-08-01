namespace BackendProject.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        

        public Order? Order { get; set; }
        public Product? Product { get; internal set; }
        public string Description { get; internal set; }
    }
}
