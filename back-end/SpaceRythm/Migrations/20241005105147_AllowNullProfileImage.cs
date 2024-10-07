using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceRythm.Migrations
{
    /// <inheritdoc />
    public partial class AllowNullProfileImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "profile_image",
                table: "Users",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "profile_image",
                keyValue: null,
                column: "profile_image",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "profile_image",
                table: "Users",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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
    }
}
