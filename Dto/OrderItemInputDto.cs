using System.ComponentModel.DataAnnotations;

public class OrderItemInputDto
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    public int Quantity { get; set; }
}
