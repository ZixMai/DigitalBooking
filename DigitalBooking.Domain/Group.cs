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
            CREATE OR REPLACE FUNCTION get_group_display_name(group_id bigint)
           RETURNS text AS $$
           DECLARE
               result text;
           BEGIN
               SELECT
                   g.prefix || '-' ||
                   (EXTRACT(YEAR FROM age(now(), (g.enrollment_year::text || '-09-01 00:00:00')::timestamp)) + 1)::text ||
                   LPAD(g.number::text, 2, '0') ||
                   g.group_type || '-' ||
                   SUBSTRING(g.enrollment_year::text FROM 3 FOR 2)
               INTO result
               FROM groups g
               WHERE g.id = group_id;
           
               RETURN result;
           END;
           $$ LANGUAGE plpgsql STABLE;
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
