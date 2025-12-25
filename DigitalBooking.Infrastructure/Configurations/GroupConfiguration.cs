using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalBooking.Infrastructure.Configurations;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("groups");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id)
            .HasColumnName("id");

        builder.Property(g => g.Prefix)
            .HasColumnName("prefix")
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(g => g.Institute)
            .HasColumnName("institute")
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(g => g.GroupType)
            .HasColumnName("group_type")
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(g => g.Speciality)
            .HasColumnName("speciality")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(g => g.EnrollmentYear)
            .HasColumnName("enrollment_year")
            .IsRequired();

        builder.Property(g => g.Number)
            .HasColumnName("number")
            .IsRequired();

        builder.Property(g => g.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();
    }
}
