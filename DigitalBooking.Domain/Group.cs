namespace DigitalBooking.Domain;

public class Group
{
    public long Id { get; set; }

    public string Prefix { get; set; } = string.Empty;
    public string Institute { get; set; } = string.Empty;
    public string GroupType { get; set; } = string.Empty;
    public string Speciality { get; set; } = string.Empty;

    public short EnrollmentYear { get; set; }
    public short Number { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<Lesson> Lessons { get; set; } = [];
    public List<StudentProfile> StudentProfiles { get; set; } = [];
    public List<Booking> Bookings { get; set; } = [];
    
    public string GetDisplayName()
    {
        /*
         IN RAW SQL
            groups.prefix || '-' ||
            (extract(YEAR FROM age(now(), (groups.enrollment_year::text || '-09-01 00:00:00')::timestamp)) + 1)::text ||
            lpad(groups.number::text, 2, '0') ||
            group_type || '-' ||
            substring(groups.enrollment_year::text FROM 3 FOR 2)
        */
        var current = DateTime.UtcNow;
        var enrollmentDate = new DateTime(EnrollmentYear, 9, 1, 0, 0, 0, DateTimeKind.Utc);
        var years = current.Year - enrollmentDate.Year;
        if (current < enrollmentDate.AddYears(years))
            years--;
        var course = years + 1;
        
        return $"{Prefix}-{course}{Number:00}{GroupType}-{EnrollmentYear % 100:00}";
    }
}
