using EcommerceApp.Data;
using EcommerceApp.Dtos;
using EcommerceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto dto)
        {
            if (_context.Users.Any(u => u.Email == dto.Email))
                return BadRequest("Email already registered");

            if (!int.TryParse(dto.Password, out int passwordInt))
                return BadRequest("Password must be an integer");

            var user = new User
            {
                Username = dto.Username ?? string.Empty,
                Email = dto.Email ?? string.Empty,
                Password = passwordInt.ToString(),
                PhoneNumber = dto.PhoneNumber ?? string.Empty,
                Role = "user"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || user.Password != dto.Password)
                return Unauthorized("Invalid credentials");

            return Ok("Login successful");
        }

        [HttpPost("admin")]
        public async Task<IActionResult> AdminLogin(LoginDto dto)
        {
            var admin = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email && u.Role == "admin");

            if (admin == null || admin.Password != dto.Password)
                return Unauthorized("Invalid admin credentials");

            return Ok("Admin login successful");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.CartItems)
                .Include(u => u.WishlistItems)
                .Include(u => u.Orders)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound("User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("User and related data deleted successfully");
        }

    }
}
