using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalBooking.Infrastructure.Configurations;

public class DisciplineConfiguration : IEntityTypeConfiguration<Discipline>
{
    public void Configure(EntityTypeBuilder<Discipline> builder)
    {
        builder.ToTable("disciplines");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasColumnName("id");

        builder.Property(d => d.Title)
            .HasColumnName("title")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();
    }
}
