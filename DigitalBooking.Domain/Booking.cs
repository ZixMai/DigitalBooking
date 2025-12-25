namespace DigitalBooking.Domain;

public class Booking
{
    public long Id { get; set; }
    public long ClassroomId { get; set; }
    public Guid PersonBookedId { get; set; }
    public Guid ResponsiblePersonId { get; set; }
    public DateTime ModifiedAt { get; set; }
    public long? LessonId { get; set; }
    public long? GroupId { get; private set; }
    public string? MeetingLink { get; set; }
    public long? BookingAssetId { get; set; }
    public DateTime BookingStart { get; set; }
    public DateTime BookingEnd { get; set; }
    public string? BookingNote { get; set; }
    public Guid? CancelledById { get; set; }

    public Classroom Classroom { get; set; } = null!;
    public User PersonBooked { get; set; } = null!;
    public User ResponsiblePerson { get; set; } = null!;
    public Lesson? Lesson { get; set; }
    public Group? Group { get; set; }
    public BookingAsset? BookingAsset { get; set; }
    public User? CancelledBy { get; set; }

    public List<Attendance> Attendance { get; set; } = [];
    public List<Notification> Notifications { get; set; } = [];
}
