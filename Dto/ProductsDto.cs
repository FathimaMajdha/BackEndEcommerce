namespace BackendProject.Dto
{
    public class ProductsDto
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public CategoryDto? Category { get; set; }
    }
}
