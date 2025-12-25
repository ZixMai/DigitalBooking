using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;
using DigitalBooking.Infrastructure.Configurations;

namespace DigitalBooking.Infrastructure;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext 
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Department> Departments { get; set; } = null!;
    public DbSet<ClassroomType> ClassroomTypes { get; set; } = null!;
    public DbSet<Classroom> Classrooms { get; set; } = null!;
    public DbSet<BookingAsset> BookingAssets { get; set; } = null!;
    public DbSet<Booking> BookingEvents { get; set; } = null!;
    public DbSet<Discipline> Disciplines { get; set; } = null!;
    public DbSet<Lesson> Lessons { get; set; } = null!;
    public DbSet<Group> Groups { get; set; } = null!;
    public DbSet<Attendance> Attendance { get; set; } = null!;
    public DbSet<StudentProfile> StudentProfiles { get; set; } = null!;
    public DbSet<TeacherProfile> TeacherProfiles { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<Post> Posts { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<LibrarySpace> LibrarySpaces { get; set; } = null!;
    public DbSet<LessonMaterial> LessonMaterials { get; set; } = null!;
        
    public DbContext(DbContextOptions<DbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension("pg_trgm");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbContext).Assembly);
    }
}
