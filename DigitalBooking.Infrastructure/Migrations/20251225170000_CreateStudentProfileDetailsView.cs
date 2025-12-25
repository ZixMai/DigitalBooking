using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateStudentProfileDetailsView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR REPLACE VIEW student_profile_details_view AS
SELECT
    u.user_email,
    u.username,
    u.fullname,
    d.title AS department_title,
    sp.contact_link,
    g.prefix || '-' ||
    (extract(YEAR FROM age(now(), (g.enrollment_year::text || '-09-01 00:00:00')::timestamp)) + 1)::text ||
    lpad(g.number::text, 2, '0') ||
    g.group_type || '-' ||
    substring(g.enrollment_year::text FROM 3 FOR 2) AS group_name,
    sp.created_at
FROM student_profile sp
JOIN users u ON u.id = sp.user_id
JOIN groups g ON g.id = sp.group_id
JOIN departments d ON d.id = u.department_id
WHERE u.is_deleted = false;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS student_profile_details_view;");
        }
    }
}
