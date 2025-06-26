namespace EcommerceApp.Dtos
{
    public class OrderRequestDto
    {
        public int UserId { get; set; }
        public required PaymentInfoDto PaymentInfo { get; set; }
    }
}
