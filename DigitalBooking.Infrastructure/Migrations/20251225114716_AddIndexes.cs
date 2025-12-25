using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:pg_trgm", ",,");

            // Trigger to keep booked_events.group_id in sync with lessons.group_id based on lesson_id
            migrationBuilder.Sql(@"CREATE OR REPLACE FUNCTION booked_events_set_group_from_lesson()
RETURNS trigger AS $$
BEGIN
    IF NEW.lesson_id IS NOT NULL THEN
        SELECT l.group_id INTO NEW.group_id
        FROM lessons l
        WHERE l.id = NEW.lesson_id;
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;");

            migrationBuilder.Sql(@"CREATE TRIGGER trg_booked_events_set_group_from_lesson
BEFORE INSERT OR UPDATE OF lesson_id ON booked_events
FOR EACH ROW
EXECUTE FUNCTION booked_events_set_group_from_lesson();");

            // Trigger to create notification when a booking is cancelled
            migrationBuilder.Sql(@"CREATE OR REPLACE FUNCTION booked_events_notify_cancel()
RETURNS trigger AS $$
DECLARE
    canceller_fullname text;
BEGIN
    IF OLD.cancelled_by_id IS NULL AND NEW.cancelled_by_id IS NOT NULL THEN
        SELECT u.fullname INTO canceller_fullname
        FROM users u
        WHERE u.id = NEW.cancelled_by_id;

        INSERT INTO notifications (receiver_id, message, booking_id, read, created_at)
        VALUES (
            NEW.person_booked_id,
            format('Бронирование #%s было отменено пользователем %s', NEW.id, coalesce(canceller_fullname, 'неизвестно')),
            NEW.id,
            FALSE,
            now()
        );
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;");

            migrationBuilder.Sql(@"CREATE TRIGGER trg_booked_events_notify_cancel
AFTER UPDATE OF cancelled_by_id ON booked_events
FOR EACH ROW
WHEN (OLD.cancelled_by_id IS NULL AND NEW.cancelled_by_id IS NOT NULL)
EXECUTE FUNCTION booked_events_notify_cancel();");

            // Trigger to create notification on comment reply
            migrationBuilder.Sql(@"CREATE OR REPLACE FUNCTION comments_notify_reply()
RETURNS trigger AS $$
DECLARE
    author_fullname text;
BEGIN
    SELECT u.fullname INTO author_fullname
    FROM users u
    WHERE u.id = NEW.creator_id;

    INSERT INTO notifications (receiver_id, message, booking_id, read, created_at)
    VALUES (
        NEW.reply_to_user_id,
        format('%s ответил(а) вам', coalesce(author_fullname, 'Пользователь')),
        NULL,
        FALSE,
        now()
    );

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;");

            migrationBuilder.Sql(@"CREATE TRIGGER trg_comments_notify_reply
AFTER INSERT ON comments
FOR EACH ROW
WHEN (NEW.reply_to_user_id IS NOT NULL)
EXECUTE FUNCTION comments_notify_reply();");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS trg_comments_notify_reply ON comments;");
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS comments_notify_reply();");

            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS trg_booked_events_notify_cancel ON booked_events;");
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS booked_events_notify_cancel();");

            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS trg_booked_events_set_group_from_lesson ON booked_events;");
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS booked_events_set_group_from_lesson();");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:pg_trgm", ",,");
        }
    }
}
