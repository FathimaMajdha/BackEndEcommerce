using EcommerceApp.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!; 
    public string PhoneNumber { get; set; } = default!;
    public bool IsBlocked { get; set; }
    public string Role { get; set; } = "user";
    public List<Order> Orders { get; set; } = new();
    public List<CartItem> CartItems { get; set; } = new();
    public List<WishlistItem> WishlistItems { get; set; } = new();
}
