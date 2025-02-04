using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;
using RideShare.Models;  // Replace with your actual namespace

namespace RideShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly RideShareContext _context;

        public PaymentController(RideShareContext context)
        {
            _context = context;
        }
       





        // POST: api/Payment
        [HttpPost]
        public async Task<IActionResult> SavePayment([FromBody] Payment payment)
        {
            if (payment == null)
            {
                return BadRequest(new { success = false, message = "Invalid payment data." });
            }

            try
            {
                // Save the payment to the database
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Payment saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error saving payment." });
            }

        }




    }
}
