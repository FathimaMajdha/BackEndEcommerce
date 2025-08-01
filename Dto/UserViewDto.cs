﻿namespace BackendProject.Dto
{
    public class UserViewDto
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public bool IsBlocked { get; set; }
    }
}
