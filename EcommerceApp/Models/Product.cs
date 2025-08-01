namespace EcommerceApp.Models;
public class Product
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Reviews { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int Category { get; set; } = default!;
    public object ProductName { get; internal set; }
}
