using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RideShare.Models;

namespace RideShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RidesController : ControllerBase
    {
        private readonly RideShareContext _context;

        public RidesController(RideShareContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ride>>> GetRidesWithDetails()
        {
            var futureRides = await _context.Rides
                .OrderBy(r => r.Date) // Sort by Date
                .Join(
                    _context.Users, // Join with Users table
                    ride => ride.DriverId, // From Rides table, DriverId column
                    user => user.Id, // From Users table, Id column
                    (ride, user) => new // Anonymous object with combined fields
                    {
                        ride.Id,
                        ride.Route,
                        ride.Date,
                        DriverName = user.Name, // Get driver name from Users table
                        ride.Price,
                        
                    })
                .ToListAsync(); // Fetch all rides first

            return Ok(futureRides);
        }







        [HttpPost("publish")]
        public async Task<IActionResult> PublishRide([FromBody] Ride ride)
        {
            if (ride == null)
            {
                return BadRequest("Ride data is null.");
            }

            // Log the received ride data for debugging
            Console.WriteLine($"Received Ride: {JsonConvert.SerializeObject(ride)}");

            try
            {
                await _context.Rides.AddAsync(ride);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Ride published successfully!" });
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error saving ride: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("myrides/{driverId}")]
        public async Task<IActionResult> GetRidesByDriverId(int driverId)
        {
            try
            {
                var rides = await _context.Rides
                    .Where(r => r.DriverId == driverId)
                    .ToListAsync();

                if (rides == null || rides.Count == 0)
                {
                    return NotFound(new { message = "No rides found for this driver." });
                }

                return Ok(rides);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving rides for driver {driverId}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }


        }





    }
}


