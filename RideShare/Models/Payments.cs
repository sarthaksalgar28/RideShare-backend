using System.ComponentModel.DataAnnotations;

public class Payment
{
    [Key]
    [Required]
    public string paymentId { get; set; }  // Razorpay payment ID, Primary Key

    [Required]
    public string status { get; set; }     // Payment status (e.g., success, failed)

    [Required]
    public int amount { get; set; }        // Paid amount (int, as it represents a numeric value)

    [Required]
    public int rideId { get; set; }        // Just the rideId, no ForeignKey reference

    public int userId { get; set; }        // User ID linked to the payment

    // Payment timestamp, default to current time
    
}
