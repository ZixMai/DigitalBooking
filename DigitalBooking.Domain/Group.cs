namespace DigitalBooking.Domain;

public class Group
{
    public long Id { get; set; }

    public string Prefix { get; set; } = string.Empty;
    public string Institute { get; set; } = string.Empty;
    public string GroupType { get; set; } = string.Empty;
    public string Speciality { get; set; } = string.Empty;

    public short EnrollmentYear { get; set; }
    public short Number { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<Lesson> Lessons { get; set; } = [];
    public List<StudentProfile> StudentProfiles { get; set; } = [];
    public List<Booking> Bookings { get; set; } = [];
}
