namespace DigitalBooking.Domain;

public class Classroom
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public long OwnerDepartmentId { get; set; }
    public short BookingSlotsLimit { get; set; } = 1;
    public short? Capacity { get; set; }
    public long ClassroomTypeId { get; set; }
    public DateTime CreatedAt { get; set; }

    public Department OwnerDepartment { get; set; } = null!;
    public ClassroomType ClassroomType { get; set; } = null!;

    public List<Booking> Bookings { get; set; } = [];
}
