using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixEnumRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "department_id",
                table: "users",
                type: "bigint",
                nullable: false,
                defaultValue: 1L,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.InsertData(
                table: "departments",
                columns: new[] { "id", "title" },
                values: new object[] { 1L, "-" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "departments",
                keyColumn: "id",
                keyValue: 1L);

            migrationBuilder.AlterColumn<long>(
                name: "department_id",
                table: "users",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValue: 1L);
        }
    }
}
