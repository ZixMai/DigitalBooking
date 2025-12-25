namespace DigitalBooking.Domain;

public class LessonMaterial
{
    public Guid Id { get; private set; }
    public long LessonId { get; set; }

    // Raw JSON stored in the database
    public string Files { get; set; } = "[]";

    public string? Message { get; set; }
    public DateTime CreatedAt { get; set; }

    public Lesson Lesson { get; set; } = null!;
}
