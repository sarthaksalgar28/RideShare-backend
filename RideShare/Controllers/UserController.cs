using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RideShare; // Ensure this namespace contains your RideShareContext
using RideShare.Models; // Ensure this namespace contains your User model
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RideShare.Models; // Make sure to include your models
using System.Threading.Tasks;

namespace RideShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly RideShareContext _context;

        public UserController(RideShareContext context)
        {
            _context = context;
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            // Find the user by id
            var user = await _context.Users.FindAsync(id);

            // If user not found, return 404
            if (user == null)
            {
                return NotFound(new { error = "User not found" });
            }

            // Return user details (excluding sensitive data like password)
            return Ok(new
            {
                user.Id,
                user.Name,
                user.Email,
                user.MobileNumber
            });
        }

        // PUT: api/user/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest(new { error = "User ID mismatch" });
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { error = "User not found" });
            }

            // Update user details
            user.Name = request.Username;
            user.Email = request.Email;
            user.MobileNumber = request.MobileNumber;

            // Save changes to the database
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Return the updated user details (200 OK)
            return Ok(new
            {
                user.Id,
                user.Name,
                user.Email,
                user.MobileNumber
            });
        }
    }



    // Model to accept user update requests
    public class UserUpdateRequest
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
    }
}
