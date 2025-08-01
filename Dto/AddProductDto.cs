namespace BackendProject.Dto
{
    public class AddProductDto
    {
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }

        public int Reviews { get; set; }
        public int CategoryId { get; set; }
    }
}
