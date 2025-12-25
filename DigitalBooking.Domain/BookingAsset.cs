namespace DigitalBooking.Domain;

public class BookingAsset
{
    public long Id { get; set; }
    public string? RRule { get; set; }
    public DateTime StartTime { get; set; }
    public TimeSpan? RepeatDuration { get; set; }
    public Guid? OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }

    public User? Owner { get; set; }
    public List<Booking> Bookings { get; set; } = [];
}
