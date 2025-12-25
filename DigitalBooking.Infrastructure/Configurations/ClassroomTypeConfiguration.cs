using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalBooking.Infrastructure.Configurations;

public class ClassroomTypeConfiguration : IEntityTypeConfiguration<ClassroomType>
{
    public void Configure(EntityTypeBuilder<ClassroomType> builder)
    {
        builder.ToTable("classroom_types");

        builder.HasKey(ct => ct.Id);

        builder.Property(ct => ct.Id)
            .HasColumnName("id");

        builder.Property(ct => ct.TypeName)
            .HasColumnName("type_name")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ct => ct.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();
    }
}
