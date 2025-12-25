namespace DigitalBooking.Domain;

public class Post
{
    public Guid Id { get; private set; }
    public string Content { get; set; } = string.Empty;

    // Raw JSON stored in the database
    public string Files { get; set; } = "[]";
    public string Tags { get; set; } = "[]";

    public long? DisciplineId { get; set; }
    public Guid CreatorId { get; set; }
    public DateTime CreatedAt { get; set; }

    public Discipline? Discipline { get; set; }
    public User Creator { get; set; } = null!;

    public List<Comment> Comments { get; set; } = [];
}
