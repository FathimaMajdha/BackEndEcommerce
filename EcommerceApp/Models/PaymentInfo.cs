using EcommerceApp.Models;

public class PaymentInfo
{
    public int Id { get; set; }
    public string CardholderName { get; set; } = default!;
    public string CardNumber { get; set; } = default!;
    public string ExpirationDate { get; set; } = default!;
    public string CVV { get; set; } = default!;
    public int OrderId { get; set; }
    public Order? Order { get; set; }
}
