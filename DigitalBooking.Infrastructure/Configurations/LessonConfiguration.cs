using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalBooking.Infrastructure.Configurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.ToTable("lessons");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .HasColumnName("id");

        builder.Property(l => l.DisciplineId)
            .HasColumnName("discipline_id")
            .IsRequired();

        builder.Property(l => l.TeacherId)
            .HasColumnName("teacher_id")
            .IsRequired();

        builder.Property(l => l.GroupId)
            .HasColumnName("group_id")
            .IsRequired();

        builder.Property(l => l.LmsLink)
            .HasColumnName("lms_link")
            .HasMaxLength(200);

        builder.Property(l => l.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.HasOne(l => l.Discipline)
            .WithMany(d => d.Lessons)
            .HasForeignKey(l => l.DisciplineId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.Teacher)
            .WithMany(u => u.LessonsTaught)
            .HasForeignKey(l => l.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.Group)
            .WithMany(g => g.Lessons)
            .HasForeignKey(l => l.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(l => new { l.DisciplineId, l.TeacherId, l.GroupId })
            .IsUnique()
            .HasDatabaseName("ux_lessons_discipline_teacher_group");
    }
}
