namespace BackendProject.Dto
{
    public class OrderAdminDetailDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int AddressId { get; set; }
        public List<OrderItemDto>? Items { get; set; }
    }


}
