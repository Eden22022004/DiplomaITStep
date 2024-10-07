using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceRythm.Migrations
{
    /// <inheritdoc />
    public partial class AzureInitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AdminLogs",
                keyColumn: "LogId",
                keyValue: 1,
                column: "Timestamp",
                value: new DateTime(2024, 10, 5, 13, 44, 3, 716, DateTimeKind.Local).AddTicks(8227));

            migrationBuilder.UpdateData(
                table: "AdminLogs",
                keyColumn: "LogId",
                keyValue: 2,
                column: "Timestamp",
                value: new DateTime(2024, 10, 5, 13, 44, 3, 716, DateTimeKind.Local).AddTicks(8231));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 1,
                column: "PostedDate",
                value: new DateTime(2024, 10, 5, 13, 44, 3, 716, DateTimeKind.Local).AddTicks(8072));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 2,
                column: "PostedDate",
                value: new DateTime(2024, 10, 5, 13, 44, 3, 716, DateTimeKind.Local).AddTicks(8076));

            migrationBuilder.UpdateData(
                table: "Followers",
                keyColumns: new[] { "FollowedUserId", "UserId" },
                keyValues: new object[] { 2, 1 },
                column: "FollowDate",
                value: new DateTime(2024, 10, 5, 13, 44, 3, 716, DateTimeKind.Local).AddTicks(8096));

            migrationBuilder.UpdateData(
                table: "Followers",
                keyColumns: new[] { "FollowedUserId", "UserId" },
                keyValues: new object[] { 1, 2 },
                column: "FollowDate",
                value: new DateTime(2024, 10, 5, 13, 44, 3, 716, DateTimeKind.Local).AddTicks(8099));

            migrationBuilder.UpdateData(
                table: "Likes",
                keyColumns: new[] { "TrackId", "UserId" },
                keyValues: new object[] { 1, 1 },
                column: "LikedDate",
                value: new DateTime(2024, 10, 5, 13, 44, 3, 716, DateTimeKind.Local).AddTicks(8170));

            migrationBuilder.UpdateData(
                table: "Likes",
                keyColumns: new[] { "TrackId", "UserId" },
                keyValues: new object[] { 2, 2 },
                column: "LikedDate",
                value: new DateTime(2024, 10, 5, 13, 44, 3, 716, DateTimeKind.Local).AddTicks(8172));

            migrationBuilder.UpdateData(
                table: "PlaylistTracks",
                keyColumns: new[] { "PlaylistId", "TrackId" },
                keyValues: new object[] { 1, 1 },
                column: "AddedDate",
                value: new DateTime(2024, 10, 5, 10, 44, 3, 716, DateTimeKind.Utc).AddTicks(8146));

            migrationBuilder.UpdateData(
                table: "PlaylistTracks",
                keyColumns: new[] { "PlaylistId", "TrackId" },
                keyValues: new object[] { 2, 2 },
                column: "AddedDate",
                value: new DateTime(2024, 10, 5, 10, 44, 3, 716, DateTimeKind.Utc).AddTicks(8148));

            migrationBuilder.UpdateData(
                table: "Playlists",
                keyColumn: "PlaylistId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 5, 13, 44, 3, 716, DateTimeKind.Local).AddTicks(8016));

            migrationBuilder.UpdateData(
                table: "Playlists",
                keyColumn: "PlaylistId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 5, 13, 44, 3, 716, DateTimeKind.Local).AddTicks(8020));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 1,
                columns: new[] { "SubscriptionEndDate", "SubscriptionStartDate" },
                values: new object[] { new DateTime(2025, 10, 5, 10, 44, 3, 716, DateTimeKind.Utc).AddTicks(8198), new DateTime(2024, 10, 5, 10, 44, 3, 716, DateTimeKind.Utc).AddTicks(8198) });

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 2,
                columns: new[] { "SubscriptionEndDate", "SubscriptionStartDate" },
                values: new object[] { new DateTime(2024, 11, 5, 10, 44, 3, 716, DateTimeKind.Utc).AddTicks(8205), new DateTime(2024, 10, 5, 10, 44, 3, 716, DateTimeKind.Utc).AddTicks(8204) });

            migrationBuilder.UpdateData(
                table: "Tracks",
                keyColumn: "TrackId",
                keyValue: 1,
                column: "UploadDate",
                value: new DateTime(2024, 10, 5, 10, 44, 3, 716, DateTimeKind.Utc).AddTicks(8047));

            migrationBuilder.UpdateData(
                table: "Tracks",
                keyColumn: "TrackId",
                keyValue: 2,
                column: "UploadDate",
                value: new DateTime(2024, 10, 5, 10, 44, 3, 716, DateTimeKind.Utc).AddTicks(8049));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "user_id",
                keyValue: 1,
                column: "date_joined",
                value: new DateTime(2024, 10, 5, 10, 44, 3, 716, DateTimeKind.Utc).AddTicks(7936));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "user_id",
                keyValue: 2,
                column: "date_joined",
                value: new DateTime(2024, 10, 5, 10, 44, 3, 716, DateTimeKind.Utc).AddTicks(7939));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AdminLogs",
                keyColumn: "LogId",
                keyValue: 1,
                column: "Timestamp",
                value: new DateTime(2024, 10, 5, 13, 40, 40, 320, DateTimeKind.Local).AddTicks(9218));

            migrationBuilder.UpdateData(
                table: "AdminLogs",
                keyColumn: "LogId",
                keyValue: 2,
                column: "Timestamp",
                value: new DateTime(2024, 10, 5, 13, 40, 40, 320, DateTimeKind.Local).AddTicks(9221));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 1,
                column: "PostedDate",
                value: new DateTime(2024, 10, 5, 13, 40, 40, 320, DateTimeKind.Local).AddTicks(9102));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 2,
                column: "PostedDate",
                value: new DateTime(2024, 10, 5, 13, 40, 40, 320, DateTimeKind.Local).AddTicks(9106));

            migrationBuilder.UpdateData(
                table: "Followers",
                keyColumns: new[] { "FollowedUserId", "UserId" },
                keyValues: new object[] { 2, 1 },
                column: "FollowDate",
                value: new DateTime(2024, 10, 5, 13, 40, 40, 320, DateTimeKind.Local).AddTicks(9121));

            migrationBuilder.UpdateData(
                table: "Followers",
                keyColumns: new[] { "FollowedUserId", "UserId" },
                keyValues: new object[] { 1, 2 },
                column: "FollowDate",
                value: new DateTime(2024, 10, 5, 13, 40, 40, 320, DateTimeKind.Local).AddTicks(9123));

            migrationBuilder.UpdateData(
                table: "Likes",
                keyColumns: new[] { "TrackId", "UserId" },
                keyValues: new object[] { 1, 1 },
                column: "LikedDate",
                value: new DateTime(2024, 10, 5, 13, 40, 40, 320, DateTimeKind.Local).AddTicks(9169));

            migrationBuilder.UpdateData(
                table: "Likes",
                keyColumns: new[] { "TrackId", "UserId" },
                keyValues: new object[] { 2, 2 },
                column: "LikedDate",
                value: new DateTime(2024, 10, 5, 13, 40, 40, 320, DateTimeKind.Local).AddTicks(9171));

            migrationBuilder.UpdateData(
                table: "PlaylistTracks",
                keyColumns: new[] { "PlaylistId", "TrackId" },
                keyValues: new object[] { 1, 1 },
                column: "AddedDate",
                value: new DateTime(2024, 10, 5, 10, 40, 40, 320, DateTimeKind.Utc).AddTicks(9145));

            migrationBuilder.UpdateData(
                table: "PlaylistTracks",
                keyColumns: new[] { "PlaylistId", "TrackId" },
                keyValues: new object[] { 2, 2 },
                column: "AddedDate",
                value: new DateTime(2024, 10, 5, 10, 40, 40, 320, DateTimeKind.Utc).AddTicks(9146));

            migrationBuilder.UpdateData(
                table: "Playlists",
                keyColumn: "PlaylistId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 5, 13, 40, 40, 320, DateTimeKind.Local).AddTicks(9020));

            migrationBuilder.UpdateData(
                table: "Playlists",
                keyColumn: "PlaylistId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 5, 13, 40, 40, 320, DateTimeKind.Local).AddTicks(9025));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 1,
                columns: new[] { "SubscriptionEndDate", "SubscriptionStartDate" },
                values: new object[] { new DateTime(2025, 10, 5, 10, 40, 40, 320, DateTimeKind.Utc).AddTicks(9191), new DateTime(2024, 10, 5, 10, 40, 40, 320, DateTimeKind.Utc).AddTicks(9191) });

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 2,
                columns: new[] { "SubscriptionEndDate", "SubscriptionStartDate" },
                values: new object[] { new DateTime(2024, 11, 5, 10, 40, 40, 320, DateTimeKind.Utc).AddTicks(9198), new DateTime(2024, 10, 5, 10, 40, 40, 320, DateTimeKind.Utc).AddTicks(9197) });

            migrationBuilder.UpdateData(
                table: "Tracks",
                keyColumn: "TrackId",
                keyValue: 1,
                column: "UploadDate",
                value: new DateTime(2024, 10, 5, 10, 40, 40, 320, DateTimeKind.Utc).AddTicks(9051));

            migrationBuilder.UpdateData(
                table: "Tracks",
                keyColumn: "TrackId",
                keyValue: 2,
                column: "UploadDate",
                value: new DateTime(2024, 10, 5, 10, 40, 40, 320, DateTimeKind.Utc).AddTicks(9053));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "user_id",
                keyValue: 1,
                column: "date_joined",
                value: new DateTime(2024, 10, 5, 10, 40, 40, 320, DateTimeKind.Utc).AddTicks(8935));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "user_id",
                keyValue: 2,
                column: "date_joined",
                value: new DateTime(2024, 10, 5, 10, 40, 40, 320, DateTimeKind.Utc).AddTicks(8938));
        }
    }
}
