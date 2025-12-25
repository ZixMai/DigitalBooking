CREATE TABLE "users" (
  "id" uuid PRIMARY KEY NOT NULL DEFAULT (uuidv7()),

  "user_email" text NOT NULL UNIQUE CHECK (char_length("user_email") <= 100),
  "username"   text NOT NULL UNIQUE CHECK (char_length("username") <= 100),
  "fullname"   text NOT NULL        CHECK (char_length("fullname") <= 100),

  "department_id" bigint NOT NULL,

  "password_hash" text NOT NULL CHECK (char_length("password_hash") <= 200),

  "created_at" timestamp NOT NULL DEFAULT (now()),
  "updated_at" timestamp DEFAULT null,

  "role" text NOT NULL DEFAULT 'STUDENT' CHECK (char_length("role") <= 100),
  "is_deleted" bool NOT NULL DEFAULT false
);

CREATE TABLE "classroom_types" (
  "id" bigserial PRIMARY KEY,
  "type_name" text NOT NULL CHECK (char_length("type_name") <= 100),
  "created_at" timestamp NOT NULL DEFAULT (now())
);

CREATE TABLE "departments" (
  "id" bigserial PRIMARY KEY,
  "title" text UNIQUE NOT NULL CHECK (char_length("title") <= 100)
);

CREATE TABLE "classrooms" (
  "id" bigserial PRIMARY KEY,
  "title" text NOT NULL CHECK (char_length("title") <= 100),
  "owner_department_id" bigint NOT NULL,
  "booking_slots_limit" smallint NOT NULL DEFAULT 1,
  "capacity" smallint,
  "classroom_type_id" bigint NOT NULL,
  "created_at" timestamp NOT NULL DEFAULT (now())
);

CREATE TABLE "booking_assets" (
  "id" bigserial PRIMARY KEY,
  "rrule" text CHECK (rrule IS NULL OR char_length("rrule") <= 100),
  "start_time" timestamp NOT NULL,
  "repeat_duration" interval,
  "owner_id" uuid,
  "created_at" timestamp NOT NULL DEFAULT (now())
);

CREATE TABLE "bookings" (
  "id" bigserial PRIMARY KEY,
  "classroom_id" bigint NOT NULL,
  "person_booked_id" uuid NOT NULL,
  "responsible_person" uuid NOT NULL,
  "modified_at" timestamp NOT NULL DEFAULT (now()),
  "lesson_id" bigint,
  "meeting_link" text CHECK (meeting_link IS NULL OR char_length("meeting_link") <= 1000),
  "booking_asset_id" bigint,
  "booking_start" timestamp NOT NULL,
  "booking_end" timestamp NOT NULL,
  "booking_note" text,
  "cancelled_by_id" uuid
);

CREATE TABLE "disciplines" (
  "id" bigserial PRIMARY KEY,
  "title" text NOT NULL CHECK (char_length("title") <= 200),
  "created_at" timestamp NOT NULL DEFAULT (now())
);

CREATE TABLE "lessons" (
  "id" bigserial PRIMARY KEY,
  "discipline_id" bigint NOT NULL,
  "teacher_id" uuid NOT NULL,
  "group_id" bigint NOT NULL,
  "lms_link" text CHECK (lms_link IS NULL OR char_length("lms_link") <= 200),
  "created_at" timestamp NOT NULL DEFAULT (now())
);

CREATE TABLE "groups" (
  "id" bigserial PRIMARY KEY,

  "prefix"     text NOT NULL CHECK (char_length("prefix") <= 30),
  "institute"  text NOT NULL CHECK (char_length("institute") <= 30),
  "group_type" text NOT NULL CHECK (char_length("group_type") <= 30),
  "speciality" text NOT NULL CHECK (char_length("speciality") <= 100),

  "enrollment_year" smallint NOT NULL,
  "number" smallint NOT NULL,

  "created_at" timestamp NOT NULL DEFAULT (now())
);

CREATE TABLE "attendance" (
  "booking_id" bigint NOT NULL,
  "user_id" uuid NOT NULL,
  "appeared" bool NOT NULL DEFAULT false,
  "created_at" timestamp NOT NULL DEFAULT (now()),
  PRIMARY KEY ("booking_id", "user_id")
);

CREATE TABLE "student_profile" (
  "id" bigserial PRIMARY KEY,
  "user_id" uuid UNIQUE NOT NULL,
  "group_id" bigint NOT NULL,
  "contact_link" text CHECK (contact_link IS NULL OR char_length("contact_link") <= 100),
  "created_at" timestamp NOT NULL DEFAULT (now())
);

CREATE TABLE "teacher_profile" (
  "id" bigserial PRIMARY KEY,
  "user_id" uuid UNIQUE NOT NULL,
  "academic_degree" text,
  "science_cloud_link" text CHECK (science_cloud_link IS NULL OR char_length("science_cloud_link") <= 1000),
  "created_at" timestamp NOT NULL DEFAULT (now())
);

CREATE TABLE "notifications" (
  "id" bigserial PRIMARY KEY,
  "receiver_id" uuid,
  "message" text,
  "booking_id" bigint,
  "read" bool NOT NULL DEFAULT false,
  "created_at" timestamp NOT NULL DEFAULT (now())
);

CREATE TABLE "posts" (
  "id" uuid PRIMARY KEY NOT NULL DEFAULT (uuidv7()),
  "content" text NOT NULL,
  "files" jsonb NOT NULL DEFAULT ('[]'::jsonb),
  "tags" jsonb NOT NULL DEFAULT ('[]'::jsonb),
  "discipline_id" bigint,
  "creator_id" uuid NOT NULL,
  "created_at" timestamp NOT NULL DEFAULT (now())
);

CREATE TABLE "comments" (
  "id" bigserial PRIMARY KEY,
  "creator_id" uuid NOT NULL,
  "message" text NOT NULL,
  "reply_to_post_id" uuid NOT NULL,
  "reply_to_id" bigint,
  "reply_to_user_id" uuid NOT NULL,
  "created_at" timestamp NOT NULL DEFAULT (now()),
  "updated_at" timestamp DEFAULT null
);

CREATE TABLE "library_space" (
  "id" uuid PRIMARY KEY NOT NULL DEFAULT (uuidv7()),
  "title" text NOT NULL CHECK (char_length("title") <= 1000),
  "creator_id" uuid NOT NULL,
  "uploaded_at" timestamp NOT NULL DEFAULT (now()),
  "preview_key" uuid NOT NULL,
  "content_key" uuid NOT NULL
);

CREATE TABLE "lesson_materials" (
  "id" uuid PRIMARY KEY NOT NULL DEFAULT (uuidv7()),
  "lesson_id" bigint NOT NULL,
  "files" jsonb NOT NULL DEFAULT ('[]'::jsonb),
  "message" text,
  "created_at" timestamp NOT NULL DEFAULT (now())
);

CREATE UNIQUE INDEX "ux_lessons_discipline_teacher_group" ON "lessons" ("discipline_id", "teacher_id", "group_id");

CREATE INDEX "ix_notifications_receiver_created_at" ON "notifications" ("receiver_id", "created_at");

CREATE INDEX "ix_posts_tags_gin" ON "posts" USING GIN ("tags");

CREATE INDEX "ix_posts_discipline_created_at" ON "posts" ("discipline_id", "created_at");

CREATE INDEX "ix_posts_created_at" ON "posts" ("created_at");

CREATE INDEX "ix_lesson_materials_lesson_created_at" ON "lesson_materials" ("lesson_id", "created_at");

ALTER TABLE "users" ADD FOREIGN KEY ("department_id") REFERENCES "departments" ("id");

ALTER TABLE "classrooms" ADD FOREIGN KEY ("owner_department_id") REFERENCES "departments" ("id");

ALTER TABLE "classrooms" ADD FOREIGN KEY ("classroom_type_id") REFERENCES "classroom_types" ("id");

ALTER TABLE "booking_assets" ADD FOREIGN KEY ("owner_id") REFERENCES "users" ("id");

ALTER TABLE "bookings" ADD FOREIGN KEY ("classroom_id") REFERENCES "classrooms" ("id");

ALTER TABLE "bookings" ADD FOREIGN KEY ("person_booked_id") REFERENCES "users" ("id");

ALTER TABLE "bookings" ADD FOREIGN KEY ("responsible_person") REFERENCES "users" ("id");

ALTER TABLE "bookings" ADD FOREIGN KEY ("lesson_id") REFERENCES "lessons" ("id");

ALTER TABLE "bookings" ADD FOREIGN KEY ("booking_asset_id") REFERENCES "booking_assets" ("id");

ALTER TABLE "bookings" ADD FOREIGN KEY ("cancelled_by_id") REFERENCES "users" ("id");

ALTER TABLE "lessons" ADD FOREIGN KEY ("discipline_id") REFERENCES "disciplines" ("id");

ALTER TABLE "lessons" ADD FOREIGN KEY ("teacher_id") REFERENCES "users" ("id");

ALTER TABLE "lessons" ADD FOREIGN KEY ("group_id") REFERENCES "groups" ("id");

ALTER TABLE "attendance" ADD FOREIGN KEY ("booking_id") REFERENCES "bookings" ("id");

ALTER TABLE "attendance" ADD FOREIGN KEY ("user_id") REFERENCES "users" ("id");

ALTER TABLE "student_profile" ADD FOREIGN KEY ("user_id") REFERENCES "users" ("id");

ALTER TABLE "student_profile" ADD FOREIGN KEY ("group_id") REFERENCES "groups" ("id");

ALTER TABLE "teacher_profile" ADD FOREIGN KEY ("user_id") REFERENCES "users" ("id");

ALTER TABLE "notifications" ADD FOREIGN KEY ("receiver_id") REFERENCES "users" ("id");

ALTER TABLE "notifications" ADD FOREIGN KEY ("booking_id") REFERENCES "bookings" ("id");

ALTER TABLE "posts" ADD FOREIGN KEY ("discipline_id") REFERENCES "disciplines" ("id");

ALTER TABLE "posts" ADD FOREIGN KEY ("creator_id") REFERENCES "users" ("id");

ALTER TABLE "comments" ADD FOREIGN KEY ("creator_id") REFERENCES "users" ("id");

ALTER TABLE "comments" ADD FOREIGN KEY ("reply_to_post_id") REFERENCES "posts" ("id");

ALTER TABLE "comments" ADD FOREIGN KEY ("reply_to_id") REFERENCES "comments" ("id");

ALTER TABLE "comments" ADD FOREIGN KEY ("reply_to_user_id") REFERENCES "users" ("id");

ALTER TABLE "library_space" ADD FOREIGN KEY ("creator_id") REFERENCES "users" ("id");

ALTER TABLE "lesson_materials" ADD FOREIGN KEY ("lesson_id") REFERENCES "lessons" ("id");
