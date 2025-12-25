using Microsoft.EntityFrameworkCore;

namespace DigitalBooking.Domain;

[Keyless]
public class StudentProfileDetailsView
{
    public string UserEmail { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Fullname { get; set; } = string.Empty;
    public string DepartmentTitle { get; set; } = string.Empty;
    public string? ContactLink { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
