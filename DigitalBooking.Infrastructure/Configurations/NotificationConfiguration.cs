using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalBooking.Infrastructure.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id)
            .HasColumnName("id");

        builder.Property(n => n.ReceiverId)
            .HasColumnName("receiver_id");

        builder.Property(n => n.Message)
            .HasColumnName("message");

        builder.Property(n => n.BookingId)
            .HasColumnName("booking_id");

        builder.Property(n => n.Read)
            .HasColumnName("read")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(n => n.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.HasOne(n => n.Receiver)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.ReceiverId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(n => n.Booking)
            .WithMany(b => b.Notifications)
            .HasForeignKey(n => n.BookingId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(n => new { n.ReceiverId, n.CreatedAt })
            .HasDatabaseName("ix_notifications_receiver_created_at");
    }
}
