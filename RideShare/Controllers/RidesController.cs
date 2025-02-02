using System;
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

        [HttpGet]
        public async Task<IActionResult> GetRides()
        {
            try
            {
                var rides = await _context.Rides.ToListAsync();
                return Ok(rides);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving rides: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}


