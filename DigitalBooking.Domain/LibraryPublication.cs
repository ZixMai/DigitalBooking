namespace DigitalBooking.Domain;

public class LibraryPublication
{
    public Guid Id { get; private set; }
    public string Title { get; set; } = string.Empty;
    public Guid CreatorId { get; set; }
    public DateTime UploadedAt { get; set; }
    public Guid PreviewKey { get; set; }
    public Guid ContentKey { get; set; }

    public User Creator { get; set; } = null!;
}
