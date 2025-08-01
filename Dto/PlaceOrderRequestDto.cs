using BackendProject.Dto;

public class PlaceOrderRequestDto
{
    public int UserId { get; set; }
    public UserAddressDto? Address { get; set; }   
    public List<OrderItemDto>? OrderItems { get; set; }
}
