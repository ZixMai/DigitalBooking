namespace DigitalBooking.Domain;

public class Notification
{
    public long Id { get; set; }
    public Guid? ReceiverId { get; set; }
    public string? Message { get; set; }
    public long? BookingId { get; set; }
    public bool Read { get; set; } = false;
    public DateTime CreatedAt { get; set; }

    public User? Receiver { get; set; }
    public Booking? Booking { get; set; }
}
