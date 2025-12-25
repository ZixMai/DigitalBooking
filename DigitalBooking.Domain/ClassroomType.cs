namespace DigitalBooking.Domain;

public class ClassroomType
{
    public long Id { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public List<Classroom> Classrooms { get; set; } = [];
}
