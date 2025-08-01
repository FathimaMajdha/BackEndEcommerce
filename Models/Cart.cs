using System.ComponentModel.DataAnnotations;

namespace BackendProject.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

       
        public User? User { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    }
}
