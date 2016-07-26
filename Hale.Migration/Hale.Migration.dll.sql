/* Using Database postgres and Connection String server=localhost;uid=hale;pwd=********;database=hale_testing */
/* 1: CreateUsersTable reverting ============================================= */

/* Beginning Transaction */
/* DeleteIndex Users () */
DROP INDEX "public"."I_Users_UsernameFullnameId";

/* DeleteTable Users */
DROP TABLE "public"."Users";

DELETE FROM "public"."VersionInfo" WHERE "Version" = 1;
/* Committing Transaction */
/* 1: CreateUsersTable reverted */

DROP TABLE "public"."VersionInfo";
/* Task completed. */
