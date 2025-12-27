using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalBooking.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("uuidv7()")
            .ValueGeneratedOnAdd();

        builder.Property(u => u.UserEmail)
            .HasColumnName("user_email")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Username)
            .HasColumnName("username")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Fullname)
            .HasColumnName("fullname")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.DepartmentId)
            .HasColumnName("department_id")
            .HasDefaultValue(1)
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .HasColumnName("password_hash")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp without time zone");

        builder.Property(u => u.Role)
            .HasColumnName("role")
            .IsRequired()
            .HasMaxLength(100)
            .HasDefaultValue("STUDENT");

        builder.Property(u => u.IsDeleted)
            .HasColumnName("is_deleted")
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasIndex(u => u.UserEmail)
            .IsUnique();

        builder.HasIndex(u => u.Username)
            .IsUnique();

        builder.HasOne(u => u.Department)
            .WithMany(d => d.Users)
            .HasForeignKey(u => u.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
