using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceRythm.Migrations
{
    /// <inheritdoc />
    public partial class FixIdentityFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.UpdateData(
                table: "AdminLogs",
                keyColumn: "LogId",
                keyValue: 1,
                column: "Timestamp",
                value: new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Local).AddTicks(6011));

            migrationBuilder.UpdateData(
                table: "AdminLogs",
                keyColumn: "LogId",
                keyValue: 2,
                column: "Timestamp",
                value: new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Local).AddTicks(6013));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 1,
                column: "PostedDate",
                value: new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Local).AddTicks(5869));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 2,
                column: "PostedDate",
                value: new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Local).AddTicks(5871));

            migrationBuilder.UpdateData(
                table: "Followers",
                keyColumns: new[] { "FollowedUserId", "UserId" },
                keyValues: new object[] { 2, 1 },
                column: "FollowDate",
                value: new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Local).AddTicks(5891));

            migrationBuilder.UpdateData(
                table: "Followers",
                keyColumns: new[] { "FollowedUserId", "UserId" },
                keyValues: new object[] { 1, 2 },
                column: "FollowDate",
                value: new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Local).AddTicks(5894));

            migrationBuilder.UpdateData(
                table: "Likes",
                keyColumns: new[] { "TrackId", "UserId" },
                keyValues: new object[] { 1, 1 },
                column: "LikedDate",
                value: new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Local).AddTicks(5934));

            migrationBuilder.UpdateData(
                table: "Likes",
                keyColumns: new[] { "TrackId", "UserId" },
                keyValues: new object[] { 2, 2 },
                column: "LikedDate",
                value: new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Local).AddTicks(5935));

            migrationBuilder.UpdateData(
                table: "PlaylistTracks",
                keyColumns: new[] { "PlaylistId", "TrackId" },
                keyValues: new object[] { 1, 1 },
                column: "AddedDate",
                value: new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Utc).AddTicks(5911));

            migrationBuilder.UpdateData(
                table: "PlaylistTracks",
                keyColumns: new[] { "PlaylistId", "TrackId" },
                keyValues: new object[] { 2, 2 },
                column: "AddedDate",
                value: new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Utc).AddTicks(5913));

            migrationBuilder.UpdateData(
                table: "Playlists",
                keyColumn: "PlaylistId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Local).AddTicks(5814));

            migrationBuilder.UpdateData(
                table: "Playlists",
                keyColumn: "PlaylistId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Local).AddTicks(5816));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 1,
                columns: new[] { "SubscriptionEndDate", "SubscriptionStartDate" },
                values: new object[] { new DateTime(2025, 10, 21, 7, 33, 11, 331, DateTimeKind.Utc).AddTicks(5958), new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Utc).AddTicks(5958) });

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 2,
                columns: new[] { "SubscriptionEndDate", "SubscriptionStartDate" },
                values: new object[] { new DateTime(2024, 11, 21, 7, 33, 11, 331, DateTimeKind.Utc).AddTicks(5980), new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Utc).AddTicks(5979) });

            migrationBuilder.UpdateData(
                table: "Tracks",
                keyColumn: "TrackId",
                keyValue: 1,
                column: "UploadDate",
                value: new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Utc).AddTicks(5843));

            migrationBuilder.UpdateData(
                table: "Tracks",
                keyColumn: "TrackId",
                keyValue: 2,
                column: "UploadDate",
                value: new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Utc).AddTicks(5846));

           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Users");

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
                column: "date_joined",
                value: new DateTime(2024, 10, 17, 10, 51, 43, 180, DateTimeKind.Utc).AddTicks(3245));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "user_id",
                keyValue: 2,
                column: "date_joined",
                value: new DateTime(2024, 10, 17, 10, 51, 43, 180, DateTimeKind.Utc).AddTicks(3249));
        }
    }
}
