using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalBooking.Infrastructure.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasColumnName("id");

        builder.Property(d => d.Title)
            .HasColumnName("title")
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(d => d.Title)
            .IsUnique();

        // Seed default department with Id = 1 and Title = "-"
        builder.HasData(new Department
        {
            Id = 1,
            Title = "-"
        });
    }
}
