using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace BackendProject.Dto
{
    public class PaymentDto


    {
        [Required]
      
        public string? razorpay_order_id { get; set; }

        [Required]
        public string? razorpay_payment_id { get; set; }
       

        [Required]
        public string? razorpay_signature { get; set; }
    }



}
