using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalBooking.Infrastructure.Configurations;

public class LibrarySpaceConfiguration : IEntityTypeConfiguration<LibrarySpace>
{
    public void Configure(EntityTypeBuilder<LibrarySpace> builder)
    {
        builder.ToTable("library_space");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("uuidv7()")
            .ValueGeneratedOnAdd();

        builder.Property(l => l.Title)
            .HasColumnName("title")
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(l => l.CreatorId)
            .HasColumnName("creator_id")
            .IsRequired();

        builder.Property(l => l.UploadedAt)
            .HasColumnName("uploaded_at")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.Property(l => l.PreviewKey)
            .HasColumnName("preview_key")
            .IsRequired();

        builder.Property(l => l.ContentKey)
            .HasColumnName("content_key")
            .IsRequired();

        builder.HasOne(l => l.Creator)
            .WithMany(u => u.LibrarySpaces)
            .HasForeignKey(l => l.CreatorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
