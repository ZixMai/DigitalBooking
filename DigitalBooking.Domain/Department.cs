namespace DigitalBooking.Domain;

public class Department
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;

    public List<User> Users { get; set; } = [];
    public List<Classroom> Classrooms { get; set; } = [];
}
