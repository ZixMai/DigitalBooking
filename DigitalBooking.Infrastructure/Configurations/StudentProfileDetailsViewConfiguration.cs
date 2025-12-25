using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalBooking.Infrastructure.Configurations;

public class StudentProfileDetailsViewConfiguration : IEntityTypeConfiguration<StudentProfileDetailsView>
{
    public void Configure(EntityTypeBuilder<StudentProfileDetailsView> builder)
    {
        builder.ToView("student_profile_details_view");
        builder.HasNoKey();

        builder.Property(v => v.UserEmail)
            .HasColumnName("user_email");

        builder.Property(v => v.Username)
            .HasColumnName("username");

        builder.Property(v => v.Fullname)
            .HasColumnName("fullname");

        builder.Property(v => v.DepartmentTitle)
            .HasColumnName("department_title");

        builder.Property(v => v.ContactLink)
            .HasColumnName("contact_link");

        builder.Property(v => v.GroupName)
            .HasColumnName("group_name");

        builder.Property(v => v.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp without time zone");
    }
}
