using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalBooking.Infrastructure.Configurations;

public class ClassroomConfiguration : IEntityTypeConfiguration<Classroom>
{
    public void Configure(EntityTypeBuilder<Classroom> builder)
    {
        builder.ToTable("classrooms");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id");

        builder.Property(c => c.Title)
            .HasColumnName("title")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.OwnerDepartmentId)
            .HasColumnName("owner_department_id")
            .IsRequired();

        builder.Property(c => c.BookingSlotsLimit)
            .HasColumnName("booking_slots_limit")
            .HasDefaultValue((short)1)
            .IsRequired();

        builder.Property(c => c.Capacity)
            .HasColumnName("capacity");

        builder.Property(c => c.ClassroomTypeId)
            .HasColumnName("classroom_type_id")
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.HasOne(c => c.OwnerDepartment)
            .WithMany(d => d.Classrooms)
            .HasForeignKey(c => c.OwnerDepartmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.ClassroomType)
            .WithMany(ct => ct.Classrooms)
            .HasForeignKey(c => c.ClassroomTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
