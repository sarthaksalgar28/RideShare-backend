﻿using System;
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
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<object>>> GetFilteredRides([FromQuery] string from, [FromQuery] string to, [FromQuery] string date)
        {
            try
            {
                var futureRides = await _context.Rides
                    .OrderBy(r => r.Date)
                    .Join(
                        _context.Users,
                        ride => ride.DriverId,
                        user => user.Id,
                        (ride, user) => new
                        {
                            ride.Id,
                            ride.Route,
                            ride.Date,
                            DriverName = user.Name,
                            ride.Price,
                            ride.Passengers,
                            BookedPassengers = _context.Payments.Count(p => p.rideId == ride.Id),
                        })
                    .Where(ride => (ride.Passengers - ride.BookedPassengers) > 0) // Filter for available seats
                    .Where(ride => ride.Route.Contains(from) && ride.Route.Contains(to)) // Filter by source and destination
                    .Where(ride => ride.Date.Contains(date)) // Filter by date
                    .Select(ride => new
                    {
                        ride.Id,
                        ride.Route,
                        ride.Date,
                        ride.DriverName,
                        ride.Price,
                        ride.Passengers,
                        RemainingPassengers = ride.Passengers - ride.BookedPassengers
                    })
                    .ToListAsync();

                return Ok(futureRides);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching filtered rides: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost("update-remaining-passenger/{rideId}")]
        public async Task<IActionResult> UpdateRemainingPassengers(int rideId)
        {
            var ride = await _context.Rides.FindAsync(rideId);
            if (ride == null)
            {
                return NotFound();
            }

            if (ride.remainingpassengers > 0)
            {
                ride.remainingpassengers -= 1;  // Decrease remaining passengers
                await _context.SaveChangesAsync();
                return Ok(ride);
            }
            else
            {
                return BadRequest("No remaining passengers.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetRidesWithDetails()
        {
            var futureRides = await _context.Rides
     .OrderBy(r => r.Date)
     .Join(
         _context.Users,
         ride => ride.DriverId,
         user => user.Id,
         (ride, user) => new
         {
             ride.Id,
             ride.Route,
             ride.Date,
             DriverName = user.Name,
             ride.Price,
             ride.Passengers,
             BookedPassengers = _context.Payments.Count(p => p.rideId == ride.Id),
         })
     .Where(ride => (ride.Passengers - ride.BookedPassengers) > 0)  // Filter for available seats
     .Select(ride => new
     {
         ride.Id,
         ride.Route,
         ride.Date,
         ride.DriverName,
         ride.Price,
         ride.Passengers,
         RemainingPassengers = ride.Passengers - ride.BookedPassengers
     })
     .ToListAsync();



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
                // Set the remaining passengers to the number of passengers
                ride.remainingpassengers = ride.Passengers;


                // Add the ride to the database
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
                // Fetching rides for the given driverId
                var rides = await _context.Rides
                    .Where(r => r.DriverId == driverId)
                    .ToListAsync();

                // Check if no rides are found
                if (rides == null || rides.Count == 0)
                {
                    return NotFound(new { message = "No rides found for this driver." });
                }

                return Ok(rides);
            }
            catch (Exception ex)
            {
                // Log the error with more details for debugging
                Console.WriteLine($"Error retrieving rides for driver {driverId}: {ex.Message}");
                // Log the stack trace for further debugging
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500, "Internal server error");
            }
        }



    }





}



