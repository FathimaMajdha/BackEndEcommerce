using BackendProject.Models;

public class WishListItem
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    public int Quantity { get; set; } = 1;

    public decimal Price { get; set; } 
    public decimal TotalPrice => Price * Quantity;

    public int WishListId { get; set; }
    public WishList? WishList { get; set; }
}
