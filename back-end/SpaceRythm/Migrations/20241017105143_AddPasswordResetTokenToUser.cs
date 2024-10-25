using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceRythm.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordResetTokenToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "password_reset_token",
                table: "Users",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "reset_token_expires",
                table: "Users",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AdminLogs",
                keyColumn: "LogId",
                keyValue: 1,
                column: "Timestamp",
                value: new DateTime(2024, 10, 17, 10, 51, 43, 180, DateTimeKind.Local).AddTicks(3485));

            migrationBuilder.UpdateData(
                table: "AdminLogs",
                keyColumn: "LogId",
                keyValue: 2,
                column: "Timestamp",
                value: new DateTime(2024, 10, 17, 10, 51, 43, 180, DateTimeKind.Local).AddTicks(3487));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 1,
                column: "PostedDate",
                value: new DateTime(2024, 10, 17, 10, 51, 43, 180, DateTimeKind.Local).AddTicks(3351));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 2,
                column: "PostedDate",
                value: new DateTime(2024, 10, 17, 10, 51, 43, 180, DateTimeKind.Local).AddTicks(3353));

            migrationBuilder.UpdateData(
                table: "Followers",
                keyColumns: new[] { "FollowedUserId", "UserId" },
                keyValues: new object[] { 2, 1 },
                column: "FollowDate",
                value: new DateTime(2024, 10, 17, 10, 51, 43, 180, DateTimeKind.Local).AddTicks(3376));

            migrationBuilder.UpdateData(
                table: "Followers",
                keyColumns: new[] { "FollowedUserId", "UserId" },
                keyValues: new object[] { 1, 2 },
                column: "FollowDate",
                value: new DateTime(2024, 10, 17, 10, 51, 43, 180, DateTimeKind.Local).AddTicks(3378));

            migrationBuilder.UpdateData(
                table: "Likes",
                keyColumns: new[] { "TrackId", "UserId" },
                keyValues: new object[] { 1, 1 },
                column: "LikedDate",
                value: new DateTime(2024, 10, 17, 10, 51, 43, 180, DateTimeKind.Local).AddTicks(3420));

            migrationBuilder.UpdateData(
                table: "Likes",
                keyColumns: new[] { "TrackId", "UserId" },
                keyValues: new object[] { 2, 2 },
                column: "LikedDate",
                value: new DateTime(2024, 10, 17, 10, 51, 43, 180, DateTimeKind.Local).AddTicks(3423));

            migrationBuilder.UpdateData(
                table: "PlaylistTracks",
                keyColumns: new[] { "PlaylistId", "TrackId" },
                keyValues: new object[] { 1, 1 },
                column: "AddedDate",
                value: new DateTime(2024, 10, 17, 10, 51, 43, 180, DateTimeKind.Utc).AddTicks(3399));

            migrationBuilder.UpdateData(
                table: "PlaylistTracks",
                keyColumns: new[] { "PlaylistId", "TrackId" },
                keyValues: new object[] { 2, 2 },
                column: "AddedDate",
                value: new DateTime(2024, 10, 17, 10, 51, 43, 180, DateTimeKind.Utc).AddTicks(3400));

            migrationBuilder.UpdateData(
                table: "Playlists",
                keyColumn: "PlaylistId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 17, 10, 51, 43, 180, DateTimeKind.Local).AddTicks(3291));

            migrationBuilder.UpdateData(
                table: "Playlists",
                keyColumn: "PlaylistId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 17, 10, 51, 43, 180, DateTimeKind.Local).AddTicks(3293));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 1,
                columns: new[] { "SubscriptionEndDate", "SubscriptionStartDate" },
                values: new object[] { new DateTime(2025, 10, 17, 10, 51, 43, 180, DateTimeKind.Utc).AddTicks(3448), new DateTime(2024, 10, 17, 10, 51, 43, 180, DateTimeKind.Utc).AddTicks(3447) });

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 2,
                columns: new[] { "SubscriptionEndDate", "SubscriptionStartDate" },
                values: new object[] { new DateTime(2024, 11, 17, 10, 51, 43, 180, DateTimeKind.Utc).AddTicks(3459), new DateTime(2024, 10, 17, 10, 51, 43, 180, DateTimeKind.Utc).AddTicks(3459) });

            migrationBuilder.UpdateData(
                table: "Tracks",
                keyColumn: "TrackId",
                keyValue: 1,
                column: "UploadDate",
                value: new DateTime(2024, 10, 17, 10, 51, 43, 180, DateTimeKind.Utc).AddTicks(3323));

            migrationBuilder.UpdateData(
                table: "Tracks",
                keyColumn: "TrackId",
                keyValue: 2,
                column: "UploadDate",
                value: new DateTime(2024, 10, 17, 10, 51, 43, 180, DateTimeKind.Utc).AddTicks(3325));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "date_joined", "password_reset_token", "reset_token_expires" },
                values: new object[] { new DateTime(2024, 10, 17, 10, 51, 43, 180, DateTimeKind.Utc).AddTicks(3245), null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "user_id",
                keyValue: 2,
                columns: new[] { "date_joined", "password_reset_token", "reset_token_expires" },
                values: new object[] { new DateTime(2024, 10, 17, 10, 51, 43, 180, DateTimeKind.Utc).AddTicks(3249), null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "password_reset_token",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "reset_token_expires",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "AdminLogs",
                keyColumn: "LogId",
                keyValue: 1,
                column: "Timestamp",
                value: new DateTime(2024, 10, 5, 13, 51, 47, 48, DateTimeKind.Local).AddTicks(5537));

            migrationBuilder.UpdateData(
                table: "AdminLogs",
                keyColumn: "LogId",
                keyValue: 2,
                column: "Timestamp",
                value: new DateTime(2024, 10, 5, 13, 51, 47, 48, DateTimeKind.Local).AddTicks(5540));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 1,
                column: "PostedDate",
                value: new DateTime(2024, 10, 5, 13, 51, 47, 48, DateTimeKind.Local).AddTicks(5390));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 2,
                column: "PostedDate",
                value: new DateTime(2024, 10, 5, 13, 51, 47, 48, DateTimeKind.Local).AddTicks(5394));

            migrationBuilder.UpdateData(
                table: "Followers",
                keyColumns: new[] { "FollowedUserId", "UserId" },
                keyValues: new object[] { 2, 1 },
                column: "FollowDate",
                value: new DateTime(2024, 10, 5, 13, 51, 47, 48, DateTimeKind.Local).AddTicks(5414));

            migrationBuilder.UpdateData(
                table: "Followers",
                keyColumns: new[] { "FollowedUserId", "UserId" },
                keyValues: new object[] { 1, 2 },
                column: "FollowDate",
                value: new DateTime(2024, 10, 5, 13, 51, 47, 48, DateTimeKind.Local).AddTicks(5417));

            migrationBuilder.UpdateData(
                table: "Likes",
                keyColumns: new[] { "TrackId", "UserId" },
                keyValues: new object[] { 1, 1 },
                column: "LikedDate",
                value: new DateTime(2024, 10, 5, 13, 51, 47, 48, DateTimeKind.Local).AddTicks(5474));

            migrationBuilder.UpdateData(
                table: "Likes",
                keyColumns: new[] { "TrackId", "UserId" },
                keyValues: new object[] { 2, 2 },
                column: "LikedDate",
                value: new DateTime(2024, 10, 5, 13, 51, 47, 48, DateTimeKind.Local).AddTicks(5477));

            migrationBuilder.UpdateData(
                table: "PlaylistTracks",
                keyColumns: new[] { "PlaylistId", "TrackId" },
                keyValues: new object[] { 1, 1 },
                column: "AddedDate",
                value: new DateTime(2024, 10, 5, 10, 51, 47, 48, DateTimeKind.Utc).AddTicks(5444));

            migrationBuilder.UpdateData(
                table: "PlaylistTracks",
                keyColumns: new[] { "PlaylistId", "TrackId" },
                keyValues: new object[] { 2, 2 },
                column: "AddedDate",
                value: new DateTime(2024, 10, 5, 10, 51, 47, 48, DateTimeKind.Utc).AddTicks(5446));

            migrationBuilder.UpdateData(
                table: "Playlists",
                keyColumn: "PlaylistId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 5, 13, 51, 47, 48, DateTimeKind.Local).AddTicks(5317));

            migrationBuilder.UpdateData(
                table: "Playlists",
                keyColumn: "PlaylistId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 5, 13, 51, 47, 48, DateTimeKind.Local).AddTicks(5326));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 1,
                columns: new[] { "SubscriptionEndDate", "SubscriptionStartDate" },
                values: new object[] { new DateTime(2025, 10, 5, 10, 51, 47, 48, DateTimeKind.Utc).AddTicks(5505), new DateTime(2024, 10, 5, 10, 51, 47, 48, DateTimeKind.Utc).AddTicks(5504) });

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 2,
                columns: new[] { "SubscriptionEndDate", "SubscriptionStartDate" },
                values: new object[] { new DateTime(2024, 11, 5, 10, 51, 47, 48, DateTimeKind.Utc).AddTicks(5513), new DateTime(2024, 10, 5, 10, 51, 47, 48, DateTimeKind.Utc).AddTicks(5512) });

            migrationBuilder.UpdateData(
                table: "Tracks",
                keyColumn: "TrackId",
                keyValue: 1,
                column: "UploadDate",
                value: new DateTime(2024, 10, 5, 10, 51, 47, 48, DateTimeKind.Utc).AddTicks(5357));

            migrationBuilder.UpdateData(
                table: "Tracks",
                keyColumn: "TrackId",
                keyValue: 2,
                column: "UploadDate",
                value: new DateTime(2024, 10, 5, 10, 51, 47, 48, DateTimeKind.Utc).AddTicks(5359));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "user_id",
                keyValue: 1,
                column: "date_joined",
                value: new DateTime(2024, 10, 5, 10, 51, 47, 48, DateTimeKind.Utc).AddTicks(5220));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "user_id",
                keyValue: 2,
                column: "date_joined",
                value: new DateTime(2024, 10, 5, 10, 51, 47, 48, DateTimeKind.Utc).AddTicks(5223));
        }
    }
}
