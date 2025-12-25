using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalBooking.Infrastructure.Configurations;

public class TeacherProfileConfiguration : IEntityTypeConfiguration<TeacherProfile>
{
    public void Configure(EntityTypeBuilder<TeacherProfile> builder)
    {
        builder.ToTable("teacher_profile");

        builder.HasKey(tp => tp.Id);

        builder.Property(tp => tp.Id)
            .HasColumnName("id");

        builder.Property(tp => tp.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(tp => tp.AcademicDegree)
            .HasColumnName("academic_degree");

        builder.Property(tp => tp.ScienceCloudLink)
            .HasColumnName("science_cloud_link")
            .HasMaxLength(1000);

        builder.Property(tp => tp.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.HasOne(tp => tp.User)
            .WithOne(u => u.TeacherProfile)
            .HasForeignKey<TeacherProfile>(tp => tp.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
