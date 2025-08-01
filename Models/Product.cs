using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendProject.Models
{
    public class Product
    {
        [Key]
        [Column("ProductId")] 
        public int ProductId { get; set; }


        public string? ProductName { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int Reviews { get; set; }

        public string? ImageUrl { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
        
    }
}
