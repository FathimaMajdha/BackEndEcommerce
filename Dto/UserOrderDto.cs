namespace BackendProject.Dto
{
    public class UserOrderDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

       

        public string Password { get; set; } = string.Empty;
        public List<OrderViewDto> Orders { get; set; } = new();
        public bool IsBlocked { get; internal set; }
    }

}
