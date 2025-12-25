using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalBooking.Infrastructure.Configurations;

public class BookingAssetConfiguration : IEntityTypeConfiguration<BookingAsset>
{
    public void Configure(EntityTypeBuilder<BookingAsset> builder)
    {
        builder.ToTable("booking_assets");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasColumnName("id");

        builder.Property(b => b.RRule)
            .HasColumnName("rrule")
            .HasMaxLength(100);

        builder.Property(b => b.StartTime)
            .HasColumnName("start_time")
            .HasColumnType("timestamp without time zone")
            .IsRequired();

        builder.Property(b => b.RepeatDuration)
            .HasColumnName("repeat_duration");

        builder.Property(b => b.OwnerId)
            .HasColumnName("owner_id");

        builder.Property(b => b.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.HasOne(b => b.Owner)
            .WithMany(u => u.BookingAssets)
            .HasForeignKey(b => b.OwnerId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
