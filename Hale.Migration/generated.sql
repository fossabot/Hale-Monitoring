/* Using Database postgres and Connection String server=localhost;uid=hale;pwd=********;database=hale_testing */
/* 1: CreateUsersTable migrating ============================================= */

/* Beginning Transaction */
/* CreateTable Users */
CREATE TABLE "public"."Users" ("Id" serial NOT NULL, "Fullname" text, "Username" text NOT NULL, "Password" char(60) NOT NULL, "OldPassword" char(60) NOT NULL, "PasswordChanged" timestamp, "Activated" boolean NOT NULL DEFAULT false, "Enabled" boolean NOT NULL DEFAULT true, "Created" timestamp NOT NULL DEFAULT (now() at time zone 'UTC'), "CreatedBy" integer NOT NULL, "Changed" timestamp NOT NULL DEFAULT (now() at time zone 'UTC'), "ChangedBy" integer NOT NULL, CONSTRAINT "PK_Users" PRIMARY KEY ("Id"));

/* CreateIndex Users (Username) */
CREATE UNIQUE INDEX "IX_Users_Username" ON "public"."Users" ("Username" ASC);

/* CreateForeignKey FK_Users_CreatedBy_Users_Id Users(CreatedBy) Users(Id) */
ALTER TABLE "public"."Users" ADD CONSTRAINT "FK_Users_CreatedBy_Users_Id" FOREIGN KEY ("CreatedBy") REFERENCES "public"."Users" ("Id") ON DELETE CASCADE ON UPDATE CASCADE;

/* CreateForeignKey FK_Users_ChangedBy_Users_Id Users(ChangedBy) Users(Id) */
ALTER TABLE "public"."Users" ADD CONSTRAINT "FK_Users_ChangedBy_Users_Id" FOREIGN KEY ("ChangedBy") REFERENCES "public"."Users" ("Id") ON DELETE CASCADE ON UPDATE CASCADE;

/* CreateIndex Users (Username, Fullname, Id) */
CREATE UNIQUE INDEX "I_Users_UsernameFullnameId" ON "public"."Users" ("Username" ASC,"Fullname" ASC,"Id" ASC);

INSERT INTO "public"."VersionInfo" ("Version","AppliedOn","Description") VALUES (1,'2016-07-26T20:38:06','CreateUsersTable');
/* Committing Transaction */
/* 1: CreateUsersTable migrated */

/* Task completed. */
