namespace DigitalBooking.Domain;

public class TeacherProfile
{
    public long Id { get; set; }
    public Guid UserId { get; set; }
    public string? AcademicDegree { get; set; }
    public string? ScienceCloudLink { get; set; }
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;
}
