namespace BackendProject.Dto
{
    public class UpdateNewProductDto
    {
        public int Id { get; set; } 
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? CategoryName { get; set; }
        public IFormFile? Image { get; set; }  
    }
}
