using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalBooking.Infrastructure.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("posts");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("uuidv7()")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Content)
            .HasColumnName("content")
            .IsRequired();

        builder.Property(p => p.Files)
            .HasColumnName("files")
            .HasColumnType("jsonb")
            .HasDefaultValueSql("'[]'::jsonb")
            .IsRequired();

        builder.Property(p => p.Tags)
            .HasColumnName("tags")
            .HasColumnType("jsonb")
            .HasDefaultValueSql("'[]'::jsonb")
            .IsRequired();

        builder.Property(p => p.DisciplineId)
            .HasColumnName("discipline_id");

        builder.Property(p => p.CreatorId)
            .HasColumnName("creator_id")
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.HasOne(p => p.Discipline)
            .WithMany(d => d.Posts)
            .HasForeignKey(p => p.DisciplineId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.Creator)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.CreatorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => p.Tags)
            .HasDatabaseName("ix_posts_tags_gin");

        builder.HasIndex(p => new { p.DisciplineId, p.CreatedAt })
            .HasDatabaseName("ix_posts_discipline_created_at");

        builder.HasIndex(p => p.CreatedAt)
            .HasDatabaseName("ix_posts_created_at");
    }
}
