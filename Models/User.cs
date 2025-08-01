using System.ComponentModel.DataAnnotations;

namespace BackendProject.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Customer";

        public bool IsBlocked { get; set; } = false;

        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
        public ICollection<CartItem>? Items { get; set; }
        public ICollection<WishList>? WishLists { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();         
        public ICollection<UserAddress>? Addresses { get; set; } = new List<UserAddress>(); 
       
    }
}
