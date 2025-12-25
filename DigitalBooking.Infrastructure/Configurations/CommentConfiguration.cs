using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalBooking.Infrastructure.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("comments", tb =>
        {
            tb.HasTrigger("trg_comments_notify_reply");
        });

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id");

        builder.Property(c => c.CreatorId)
            .HasColumnName("creator_id")
            .IsRequired();

        builder.Property(c => c.Message)
            .HasColumnName("message")
            .IsRequired();

        builder.Property(c => c.ReplyToPostId)
            .HasColumnName("reply_to_post_id")
            .IsRequired();

        builder.Property(c => c.ReplyToId)
            .HasColumnName("reply_to_id");

        builder.Property(c => c.ReplyToUserId)
            .HasColumnName("reply_to_user_id")
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp without time zone");

        builder.HasOne(c => c.Creator)
            .WithMany(u => u.CommentsCreated)
            .HasForeignKey(c => c.CreatorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.ReplyToPost)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.ReplyToPostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.ReplyTo)
            .WithMany(c2 => c2.Replies)
            .HasForeignKey(c => c.ReplyToId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(c => c.ReplyToUser)
            .WithMany(u => u.CommentsRepliedToUser)
            .HasForeignKey(c => c.ReplyToUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
