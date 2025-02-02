using Microsoft.AspNetCore.Mvc;
using RideShare.Models;

namespace RideShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignupController : ControllerBase
    {
        private readonly RideShareContext _context;

        // Constructor to inject the DbContext
        public SignupController(RideShareContext context)
        {
            _context = context;
        }


        [HttpPost]
        public IActionResult Post(User user)
        {
            if (user == null)
            {
                return BadRequest("User  data is null.");
            }

            // Add the user to the database
            _context.Users.Add(user);

            // Save changes to the database
            _context.SaveChanges();

            return Ok("User  registered successfully.");
        }
    }
}

