using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalBooking.Infrastructure.Configurations;

public class LessonMaterialConfiguration : IEntityTypeConfiguration<LessonMaterial>
{
    public void Configure(EntityTypeBuilder<LessonMaterial> builder)
    {
        builder.ToTable("lesson_materials");

        builder.HasKey(lm => lm.Id);

        builder.Property(lm => lm.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("uuidv7()")
            .ValueGeneratedOnAdd();

        builder.Property(lm => lm.LessonId)
            .HasColumnName("lesson_id")
            .IsRequired();

        builder.Property(lm => lm.Files)
            .HasColumnName("files")
            .HasColumnType("jsonb")
            .HasDefaultValueSql("'[]'::jsonb")
            .IsRequired();

        builder.Property(lm => lm.Message)
            .HasColumnName("message");

        builder.Property(lm => lm.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.HasOne(lm => lm.Lesson)
            .WithMany(l => l.LessonMaterials)
            .HasForeignKey(lm => lm.LessonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(lm => new { lm.LessonId, lm.CreatedAt })
            .HasDatabaseName("ix_lesson_materials_lesson_created_at");
    }
}
