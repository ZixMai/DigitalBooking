using System.ComponentModel.DataAnnotations;

namespace DigitalBooking.Domain;

public class User
{
    [Key]
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}