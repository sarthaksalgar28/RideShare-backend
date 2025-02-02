using Microsoft.AspNetCore.Mvc;
using RideShare.Models; // Make sure to include the namespace for your DbContext

namespace RideShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly RideShareContext _context;

        // Constructor to inject the DbContext
        public ContactController(RideShareContext context)
        {
            _context = context;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitContactForm([FromBody] ContactForm model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Message))
            {
                return BadRequest("All fields are required.");
            }

            // Save the contact form data to the database
            await _context.ContactForms.AddAsync(model);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Your message has been sent successfully!" });
        }
    }
}

