using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "bookings",
                newName: "booked_events");

            migrationBuilder.AddColumn<long>(
                name: "group_id",
                table: "booked_events",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "cancelled_at",
                table: "booked_events",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_booked_events_group_end",
                table: "booked_events",
                columns: new[] { "group_id", "booking_end" });

            migrationBuilder.CreateIndex(
                name: "ix_booked_events_active_classroom_end",
                table: "booked_events",
                columns: new[] { "classroom_id", "booking_end" },
                filter: "cancelled_at IS NULL");

            migrationBuilder.CreateIndex(
                name: "ix_booked_events_active_person_booked_end",
                table: "booked_events",
                columns: new[] { "person_booked_id", "booking_end" },
                filter: "cancelled_at IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_booked_events_group_end",
                table: "booked_events");

            migrationBuilder.DropIndex(
                name: "ix_booked_events_active_classroom_end",
                table: "booked_events");

            migrationBuilder.DropIndex(
                name: "ix_booked_events_active_person_booked_end",
                table: "booked_events");

            migrationBuilder.DropColumn(
                name: "group_id",
                table: "booked_events");

            migrationBuilder.DropColumn(
                name: "cancelled_at",
                table: "booked_events");

            migrationBuilder.RenameTable(
                name: "booked_events",
                newName: "bookings");
        }
    }
}
