using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RideShare.Models; // Ensure this namespace contains your Driver model
using System.Threading.Tasks;

namespace RideShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly RideShareContext _context;

        public DriverController(RideShareContext context)
        {
            _context = context;
        }

        // GET: api/driver/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDriver(int id)
        {
            // Find the driver by id
            var driver = await _context.Users.FindAsync(id); // Assuming drivers are stored in the Users table

            // If driver not found, return 404
            if (driver == null)
            {
                return NotFound(new { error = "Driver not found" });
            }

            // Return driver details (excluding sensitive data like password)
            return Ok(new
            {
                driver.Id,
                driver.Name,
                driver.Email,
                driver.MobileNumber,
                driver.CarNumber,
                driver.LicenseNumber,
                driver.CardLastFour
            });
        }

        // PUT: api/driver/{id}
        // PUT: api/driver/{id}
        // PUT: api/driver/{id}
        // PUT: api/driver/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDriver(int id, [FromBody] DriverUpdateRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest(new { error = "Driver ID mismatch" });
            }

            var driver = await _context.Users.FindAsync(id);
            if (driver == null)
            {
                return NotFound(new { error = "Driver not found" });
            }

            // Use reflection to update only provided fields
            foreach (var prop in typeof(DriverUpdateRequest).GetProperties())
            {
                var newValue = prop.GetValue(request);
                if (newValue != null && !string.IsNullOrEmpty(newValue.ToString()))
                {
                    var driverProp = typeof(User).GetProperty(prop.Name);
                    if (driverProp != null && driverProp.CanWrite)
                    {
                        driverProp.SetValue(driver, newValue);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return Ok(driver);
        }

    }
}





// Model to accept driver update requests
public class DriverUpdateRequest
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? MobileNumber { get; set; }
    public string? CarNumber { get; set; }
    public string? LicenseNumber { get; set; }
    public string? CardLastFour { get; set; }

}