namespace BackendProject.Models
{
    public class UserAddress
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string StreetName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string HomeAddress { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public User? User { get; internal set; }
    }
}
