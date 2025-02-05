public partial class Ride
{
    public int Id { get; set; }
    public int DriverId { get; set; }
    public string Route { get; set; } = null!;
    public string Date { get; set; } = null!;
    public decimal Price { get; set; }
    public int Passengers { get; set; }
    public int remainingpassengers { get; set; }  // Keep this

    // Remove this line:
    // public int? RemainingPassengers { get; set; }
}
