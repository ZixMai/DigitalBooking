namespace DigitalBooking.Domain;

public class Discipline
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public List<Lesson> Lessons { get; set; } = [];
    public List<Post> Posts { get; set; } = [];
}
