namespace BackendProject.Dto
{
    public class AddNewAddressDto
    {
        public required string StreetName { get; set; }
        public required string City { get; set; }
        public required string HomeAddress { get; set; }
        public required string CustomerPhone { get; set; }
        public required string PostalCode { get; set; }
    }

}
