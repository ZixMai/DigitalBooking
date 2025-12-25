namespace DigitalBooking.Domain;

public class User
{
    public Guid Id { get; private set; }

    public string UserEmail { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Fullname { get; set; } = string.Empty;

    public long DepartmentId { get; set; } = 1;
    public Department Department { get; set; } = null!;

    public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public string Role { get; set; } = "STUDENT";
    public bool IsDeleted { get; set; } = false;

    public List<BookingAsset> BookingAssets { get; set; } = [];
    public List<Booking> BookingsAsPersonBooked { get; set; } = [];
    public List<Booking> BookingsAsResponsiblePerson { get; set; } = [];
    public List<Booking> BookingsCancelled { get; set; } = [];

    public List<Lesson> LessonsTaught { get; set; } = [];
    public List<Attendance> AttendanceRecords { get; set; } = [];

    public StudentProfile? StudentProfile { get; set; }
    public TeacherProfile? TeacherProfile { get; set; }

    public List<Notification> Notifications { get; set; } = [];

    public List<Post> Posts { get; set; } = [];
    public List<Comment> CommentsCreated { get; set; } = [];
    public List<Comment> CommentsRepliedToUser { get; set; } = [];

    public List<LibrarySpace> LibrarySpaces { get; set; } = [];
}
