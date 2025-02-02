using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RideShare.Models;
using System.Threading.Tasks;

namespace RideShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly RideShareContext _context;

        public AuthController(RideShareContext context)
        {
            _context = context;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { error = "Email and password are required." });
            }

            // Retrieve the user from the database
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.Password == request.Password); // Use hashed passwords in production!

            if (user == null)
            {
                return Unauthorized(new { error = "Invalid email or password." });
            }

            // Return user information excluding the password
            return Ok(new
            {
                user.Id,
                user.Name,
                user.Email,
                user.Role, // Include the role in the response
                user.CarNumber,
                user.LicenseNumber,
                user.CardLastFour
            });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        // Optionally, you can add Role here if needed
        //public string Role { get; set; } // Uncomment if you want to send role during login
    }
}