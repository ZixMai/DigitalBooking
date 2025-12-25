using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalBooking.Infrastructure.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("booked_events");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasColumnName("id");

        builder.Property(b => b.ClassroomId)
            .HasColumnName("classroom_id")
            .IsRequired();

        builder.Property(b => b.PersonBookedId)
            .HasColumnName("person_booked_id")
            .IsRequired();

        builder.Property(b => b.ResponsiblePersonId)
            .HasColumnName("responsible_person")
            .IsRequired();

        builder.Property(b => b.ModifiedAt)
            .HasColumnName("modified_at")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.Property(b => b.LessonId)
            .HasColumnName("lesson_id");

        builder.Property(b => b.GroupId)
            .HasColumnName("group_id");

        builder.Property(b => b.MeetingLink)
            .HasColumnName("meeting_link")
            .HasMaxLength(1000);

        builder.Property(b => b.BookingAssetId)
            .HasColumnName("booking_asset_id");

        builder.Property(b => b.BookingStart)
            .HasColumnName("booking_start")
            .HasColumnType("timestamp without time zone")
            .IsRequired();

        builder.Property(b => b.BookingEnd)
            .HasColumnName("booking_end")
            .HasColumnType("timestamp without time zone")
            .IsRequired();

        builder.Property(b => b.BookingNote)
            .HasColumnName("booking_note");

        builder.Property(b => b.CancelledById)
            .HasColumnName("cancelled_by_id");

        builder.HasIndex(b => new { b.GroupId, b.BookingEnd })
            .HasDatabaseName("ix_booked_events_group_end");

        builder.HasIndex(b => new { b.ClassroomId, b.BookingEnd })
            .HasDatabaseName("ix_booked_events_active_classroom_end")
            .HasFilter("cancelled_at IS NULL");

        builder.HasIndex(b => new { b.PersonBookedId, b.BookingEnd })
            .HasDatabaseName("ix_booked_events_active_person_booked_end");

        builder.HasOne(b => b.Classroom)
            .WithMany(c => c.Bookings)
            .HasForeignKey(b => b.ClassroomId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.PersonBooked)
            .WithMany(u => u.BookingsAsPersonBooked)
            .HasForeignKey(b => b.PersonBookedId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.ResponsiblePerson)
            .WithMany(u => u.BookingsAsResponsiblePerson)
            .HasForeignKey(b => b.ResponsiblePersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.Lesson)
            .WithMany(l => l.Bookings)
            .HasForeignKey(b => b.LessonId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.Group)
            .WithMany(g => g.Bookings)
            .HasForeignKey(b => b.GroupId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.BookingAsset)
            .WithMany(ba => ba.Bookings)
            .HasForeignKey(b => b.BookingAssetId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.CancelledBy)
            .WithMany(u => u.BookingsCancelled)
            .HasForeignKey(b => b.CancelledById)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
