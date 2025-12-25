namespace DigitalBooking.Domain;

public class Attendance
{
    public long BookingId { get; set; }
    public Guid UserId { get; set; }
    public bool Appeared { get; set; } = false;
    public DateTime CreatedAt { get; set; }

    public Booking Booking { get; set; } = null!;
    public User User { get; set; } = null!;
}
