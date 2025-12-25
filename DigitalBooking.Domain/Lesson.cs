namespace DigitalBooking.Domain;

public class Lesson
{
    public long Id { get; set; }
    public long DisciplineId { get; set; }
    public Guid TeacherId { get; set; }
    public long GroupId { get; set; }
    public string? LmsLink { get; set; }
    public DateTime CreatedAt { get; set; }

    public Discipline Discipline { get; set; } = null!;
    public User Teacher { get; set; } = null!;
    public Group Group { get; set; } = null!;

    public List<Booking> Bookings { get; set; } = [];
    public List<LessonMaterial> LessonMaterials { get; set; } = [];
}
