namespace BackendProject.Dto
{
    public class OrderViewDto
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> Items { get; set; }


        public string? DeliveryStatus { get; set; }
        public string? PaymentStatus { get; set; }

        
    }



}
