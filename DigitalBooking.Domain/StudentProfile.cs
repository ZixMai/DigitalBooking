namespace DigitalBooking.Domain;

public class StudentProfile
{
    public long Id { get; set; }
    public Guid UserId { get; set; }
    public long GroupId { get; set; }
    public string? ContactLink { get; set; }
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;
    public Group Group { get; set; } = null!;
}
