namespace DigitalBooking.Domain;

public class Comment
{
    public long Id { get; set; }
    public Guid CreatorId { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid ReplyToPostId { get; set; }
    public long? ReplyToId { get; set; }
    public Guid ReplyToUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public User Creator { get; set; } = null!;
    public Post ReplyToPost { get; set; } = null!;
    public Comment? ReplyTo { get; set; }
    public User ReplyToUser { get; set; } = null!;

    public List<Comment> Replies { get; set; } = [];
}
