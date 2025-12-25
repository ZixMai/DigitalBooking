using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalBooking.Infrastructure.Configurations;

public class StudentProfileConfiguration : IEntityTypeConfiguration<StudentProfile>
{
    public void Configure(EntityTypeBuilder<StudentProfile> builder)
    {
        builder.ToTable("student_profile");

        builder.HasKey(sp => sp.Id);

        builder.Property(sp => sp.Id)
            .HasColumnName("id");

        builder.Property(sp => sp.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(sp => sp.GroupId)
            .HasColumnName("group_id")
            .IsRequired();

        builder.Property(sp => sp.ContactLink)
            .HasColumnName("contact_link")
            .HasMaxLength(100);

        builder.Property(sp => sp.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.HasOne(sp => sp.User)
            .WithOne(u => u.StudentProfile)
            .HasForeignKey<StudentProfile>(sp => sp.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sp => sp.Group)
            .WithMany(g => g.StudentProfiles)
            .HasForeignKey(sp => sp.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
