using System;
using System.Collections.Generic;

namespace RideShare.Models
{
    public partial class User
    {

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string? Email { get; set; }
        public string Password { get; set; } = null!;
        public string CarNumber { get; set; } = null!;
        public string LicenseNumber { get; set; } = null!;
        public string CardLastFour { get; set; } = null!;

        public string MobileNumber { get; set; } = null!;





    }
}
