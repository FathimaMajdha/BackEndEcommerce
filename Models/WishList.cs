using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendProject.Models
{
    public class WishList
    {
        [Key]
        public int Id { get; set; }


        public int UserId { get; set; }
        public User? User { get; set; }
        public List<WishListItem> Items { get; set; } = new List<WishListItem>();

       


    }
}