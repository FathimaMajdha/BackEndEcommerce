using BackendProject.Dto;

public class OrderAdminViewDto
{
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal Total { get; set; }
    public string OrderStatus { get; set; } = string.Empty;
    public List<OrderItemDto> Items { get; set; } = new();
}
