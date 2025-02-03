using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RideShare; // Your RideShareContext namespace
using RideShare.Models; // Ensure this contains your User model
using System.Threading.Tasks;

namespace RideShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerifyEmailController : ControllerBase
    {
        private readonly RideShareContext _context;

        public VerifyEmailController(RideShareContext context)
        {
            _context = context;
        }

        // POST: api/verify-email
        [HttpPost]
        public async Task<IActionResult> VerifyEmail([FromBody] EmailRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest(new { error = "Email is required." });
            }

            // Find user by email
            var user = await _context.Users
                                      .FirstOrDefaultAsync(u => u.Email == request.Email);

            // If user not found, return 404
            if (user == null)
            {
                return NotFound(new { error = "No user found with this email address." });
            }

            // If email is found, return a success message
            return Ok(new { message = "Email exists. You can proceed to update the password." });
        }
    }

    // New Request model for verifying email
    public class EmailRequest
    {
        public string Email { get; set; }
    }
}
