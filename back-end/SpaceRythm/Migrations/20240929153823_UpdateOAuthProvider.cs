using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SpaceRythm.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOAuthProvider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "oauth_provider",
                table: "Users",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Tracks",
                columns: new[] { "TrackId", "ArtistId", "Description", "Duration", "FilePath", "Genre", "Tags", "Title", "UploadDate", "UserId" },
                values: new object[,]
                {
                    { 1, 1, "This is track one", new TimeSpan(0, 0, 3, 0, 0), "/tracks/track1.mp3", "Pop", "tag1,tag2", "Track One", new DateTime(2024, 9, 29, 15, 38, 22, 961, DateTimeKind.Utc).AddTicks(3751), null },
                    { 2, 2, "This is track two", new TimeSpan(0, 0, 4, 0, 0), "/tracks/track2.mp3", "Rock", "tag3,tag4", "Track Two", new DateTime(2024, 9, 29, 15, 38, 22, 961, DateTimeKind.Utc).AddTicks(3754), null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "user_id", "biography", "date_joined", "email", "IsAdmin", "is_email_confirmed", "oauth_provider", "password_hash", "profile_image", "username" },
                values: new object[,]
                {
                    { 1, "Biography of user 1", new DateTime(2024, 9, 29, 15, 38, 22, 961, DateTimeKind.Utc).AddTicks(3534), "user1@example.com", false, true, null, "hashedpassword1", "avatar1.png", "user1" },
                    { 2, "Biography of user 2", new DateTime(2024, 9, 29, 15, 38, 22, 961, DateTimeKind.Utc).AddTicks(3538), "user2@example.com", false, true, null, "hashedpassword2", "avatar2.png", "user2" }
                });

            migrationBuilder.InsertData(
                table: "AdminLogs",
                columns: new[] { "LogId", "ActionType", "AdminId", "TargetId", "Timestamp" },
                values: new object[,]
                {
                    { 1, "UserCreated", 1, 1, new DateTime(2024, 9, 29, 18, 38, 22, 961, DateTimeKind.Local).AddTicks(3901) },
                    { 2, "TrackUploaded", 1, 1, new DateTime(2024, 9, 29, 18, 38, 22, 961, DateTimeKind.Local).AddTicks(3905) }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "CommentId", "Content", "PostedDate", "TrackId", "UserId" },
                values: new object[,]
                {
                    { 1, "Great track!", new DateTime(2024, 9, 29, 18, 38, 22, 961, DateTimeKind.Local).AddTicks(3778), 1, 2 },
                    { 2, "I love this!", new DateTime(2024, 9, 29, 18, 38, 22, 961, DateTimeKind.Local).AddTicks(3782), 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "Followers",
                columns: new[] { "FollowedUserId", "UserId", "FollowDate" },
                values: new object[,]
                {
                    { 2, 1, new DateTime(2024, 9, 29, 18, 38, 22, 961, DateTimeKind.Local).AddTicks(3800) },
                    { 1, 2, new DateTime(2024, 9, 29, 18, 38, 22, 961, DateTimeKind.Local).AddTicks(3803) }
                });

            migrationBuilder.InsertData(
                table: "Likes",
                columns: new[] { "TrackId", "UserId", "LikedDate" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 9, 29, 18, 38, 22, 961, DateTimeKind.Local).AddTicks(3842) },
                    { 2, 2, new DateTime(2024, 9, 29, 18, 38, 22, 961, DateTimeKind.Local).AddTicks(3845) }
                });

            migrationBuilder.InsertData(
                table: "Playlists",
                columns: new[] { "PlaylistId", "CreatedDate", "Description", "IsPublic", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 9, 29, 18, 38, 22, 961, DateTimeKind.Local).AddTicks(3720), "This is my first playlist", true, "My First Playlist", 1 },
                    { 2, new DateTime(2024, 9, 29, 18, 38, 22, 961, DateTimeKind.Local).AddTicks(3725), "A playlist for relaxation", true, "Chill Vibes", 2 }
                });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "SubscriptionEndDate", "SubscriptionStartDate", "Type", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 29, 15, 38, 22, 961, DateTimeKind.Utc).AddTicks(3867), new DateTime(2024, 9, 29, 15, 38, 22, 961, DateTimeKind.Utc).AddTicks(3866), 1, 1 },
                    { 2, new DateTime(2024, 10, 29, 15, 38, 22, 961, DateTimeKind.Utc).AddTicks(3875), new DateTime(2024, 9, 29, 15, 38, 22, 961, DateTimeKind.Utc).AddTicks(3875), 0, 2 }
                });

            migrationBuilder.InsertData(
                table: "PlaylistTracks",
                columns: new[] { "PlaylistId", "TrackId", "AddedDate" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 9, 29, 15, 38, 22, 961, DateTimeKind.Utc).AddTicks(3822) },
                    { 2, 2, new DateTime(2024, 9, 29, 15, 38, 22, 961, DateTimeKind.Utc).AddTicks(3823) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AdminLogs",
                keyColumn: "LogId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AdminLogs",
                keyColumn: "LogId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Followers",
                keyColumns: new[] { "FollowedUserId", "UserId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "Followers",
                keyColumns: new[] { "FollowedUserId", "UserId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "Likes",
                keyColumns: new[] { "TrackId", "UserId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "Likes",
                keyColumns: new[] { "TrackId", "UserId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "PlaylistTracks",
                keyColumns: new[] { "PlaylistId", "TrackId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "PlaylistTracks",
                keyColumns: new[] { "PlaylistId", "TrackId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Playlists",
                keyColumn: "PlaylistId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Playlists",
                keyColumn: "PlaylistId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "TrackId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "TrackId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "user_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "user_id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "oauth_provider",
                keyValue: null,
                column: "oauth_provider",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "oauth_provider",
                table: "Users",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
