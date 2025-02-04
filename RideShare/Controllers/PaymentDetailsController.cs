using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RideShare.Models;

namespace RideShare.Controllers
{
    public class PaymentDetailsController : Controller
    {
        private readonly RideShareContext _context;

        public PaymentDetailsController(RideShareContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetDetails(int userId)
        {
            // Fetch payments and join with rides based on rideId
            var paymentsWithRides = await _context.Payments
                .Where(p => p.userId == userId)
                .Join(
                    _context.Rides, // Join with the Rides table
                    payment => payment.rideId, // Match rideId from Payments table
                    ride => ride.Id, // Match Id from Rides table
                    (payment, ride) => new
                    {
                        payment.paymentId,
                        payment.amount,
                        payment.status,
                        ride.Route,
                        ride.DriverId,
                        ride.Date
                    })
                .ToListAsync(); // Execute the query and retrieve the data

            // If no payments found, return 404
            if (paymentsWithRides == null || !paymentsWithRides.Any())
            {
                return NotFound(new { error = "Payment details not found for the given userId" });
            }

            // Return the combined payment and ride details
            return Ok(paymentsWithRides);
        }
    }
}
