using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RideShare; // Your RideShareContext namespace
using RideShare.Models; // Ensure this contains your User model
using System.Threading.Tasks;

namespace RideShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdatePasswordController : ControllerBase
    {
        private readonly RideShareContext _context;

        public UpdatePasswordController(RideShareContext context)
        {
            _context = context;
        }

        // POST: api/update-password
        [HttpPost]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.NewPassword))
            {
                return BadRequest(new { error = "Email and password are required." });
            }

            // Find user by email
            var user = await _context.Users
                                      .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return NotFound(new { error = "No user found with this email address." });
            }

            // Update the password
            user.Password = request.NewPassword;  // Consider hashing the password before saving to DB

            try
            {
                // Save the changes in the database
                await _context.SaveChangesAsync();

                return Ok(new { message = "Password updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while updating the password." });
            }
        }
    }

    // Request model for updating password
    public class UpdatePasswordRequest
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}
