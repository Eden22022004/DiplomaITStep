using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceRythm.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyStamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AdminLogs",
                keyColumn: "LogId",
                keyValue: 1,
                column: "Timestamp",
                value: new DateTime(2024, 10, 21, 8, 3, 20, 279, DateTimeKind.Local).AddTicks(9426));

            migrationBuilder.UpdateData(
                table: "AdminLogs",
                keyColumn: "LogId",
                keyValue: 2,
                column: "Timestamp",
                value: new DateTime(2024, 10, 21, 8, 3, 20, 279, DateTimeKind.Local).AddTicks(9428));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 1,
                column: "PostedDate",
                value: new DateTime(2024, 10, 21, 8, 3, 20, 279, DateTimeKind.Local).AddTicks(9302));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 2,
                column: "PostedDate",
                value: new DateTime(2024, 10, 21, 8, 3, 20, 279, DateTimeKind.Local).AddTicks(9304));

            migrationBuilder.UpdateData(
                table: "Followers",
                keyColumns: new[] { "FollowedUserId", "UserId" },
                keyValues: new object[] { 2, 1 },
                column: "FollowDate",
                value: new DateTime(2024, 10, 21, 8, 3, 20, 279, DateTimeKind.Local).AddTicks(9321));

            migrationBuilder.UpdateData(
                table: "Followers",
                keyColumns: new[] { "FollowedUserId", "UserId" },
                keyValues: new object[] { 1, 2 },
                column: "FollowDate",
                value: new DateTime(2024, 10, 21, 8, 3, 20, 279, DateTimeKind.Local).AddTicks(9323));

            migrationBuilder.UpdateData(
                table: "Likes",
                keyColumns: new[] { "TrackId", "UserId" },
                keyValues: new object[] { 1, 1 },
                column: "LikedDate",
                value: new DateTime(2024, 10, 21, 8, 3, 20, 279, DateTimeKind.Local).AddTicks(9366));

            migrationBuilder.UpdateData(
                table: "Likes",
                keyColumns: new[] { "TrackId", "UserId" },
                keyValues: new object[] { 2, 2 },
                column: "LikedDate",
                value: new DateTime(2024, 10, 21, 8, 3, 20, 279, DateTimeKind.Local).AddTicks(9367));

            migrationBuilder.UpdateData(
                table: "PlaylistTracks",
                keyColumns: new[] { "PlaylistId", "TrackId" },
                keyValues: new object[] { 1, 1 },
                column: "AddedDate",
                value: new DateTime(2024, 10, 21, 8, 3, 20, 279, DateTimeKind.Utc).AddTicks(9343));

            migrationBuilder.UpdateData(
                table: "PlaylistTracks",
                keyColumns: new[] { "PlaylistId", "TrackId" },
                keyValues: new object[] { 2, 2 },
                column: "AddedDate",
                value: new DateTime(2024, 10, 21, 8, 3, 20, 279, DateTimeKind.Utc).AddTicks(9344));

            migrationBuilder.UpdateData(
                table: "Playlists",
                keyColumn: "PlaylistId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 8, 3, 20, 279, DateTimeKind.Local).AddTicks(9243));

            migrationBuilder.UpdateData(
                table: "Playlists",
                keyColumn: "PlaylistId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 8, 3, 20, 279, DateTimeKind.Local).AddTicks(9245));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 1,
                columns: new[] { "SubscriptionEndDate", "SubscriptionStartDate" },
                values: new object[] { new DateTime(2025, 10, 21, 8, 3, 20, 279, DateTimeKind.Utc).AddTicks(9390), new DateTime(2024, 10, 21, 8, 3, 20, 279, DateTimeKind.Utc).AddTicks(9390) });

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 2,
                columns: new[] { "SubscriptionEndDate", "SubscriptionStartDate" },
                values: new object[] { new DateTime(2024, 11, 21, 8, 3, 20, 279, DateTimeKind.Utc).AddTicks(9402), new DateTime(2024, 10, 21, 8, 3, 20, 279, DateTimeKind.Utc).AddTicks(9401) });

            migrationBuilder.UpdateData(
                table: "Tracks",
                keyColumn: "TrackId",
                keyValue: 1,
                column: "UploadDate",
                value: new DateTime(2024, 10, 21, 8, 3, 20, 279, DateTimeKind.Utc).AddTicks(9273));

            migrationBuilder.UpdateData(
                table: "Tracks",
                keyColumn: "TrackId",
                keyValue: 2,
                column: "UploadDate",
                value: new DateTime(2024, 10, 21, 8, 3, 20, 279, DateTimeKind.Utc).AddTicks(9275));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "date_joined", "SecurityStamp" },
                values: new object[] { "4a376fdd-c6d0-407a-8287-41df07759431", new DateTime(2024, 10, 21, 8, 3, 20, 279, DateTimeKind.Utc).AddTicks(9196), "971a4ed5-4867-4681-b74d-3f4d30e184f7" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "user_id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "date_joined", "SecurityStamp" },
                values: new object[] { "a6e0f557-f5e5-4308-b1ee-0519d04ec7b5", new DateTime(2024, 10, 21, 8, 3, 20, 279, DateTimeKind.Utc).AddTicks(9202), "08ae8d97-f992-4264-bd79-f0e4307e62af" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "date_joined", "SecurityStamp" },
                values: new object[] { "714eaac4-97b4-49aa-a32e-2e8d3a4959d3", new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Utc).AddTicks(5729), "dd275890-3b4d-439b-82df-c5cfb4b14553" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "user_id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "date_joined", "SecurityStamp" },
                values: new object[] { "7bdd07af-c747-4f80-97dd-20ff544e38ea", new DateTime(2024, 10, 21, 7, 33, 11, 331, DateTimeKind.Utc).AddTicks(5761), "233bf4d2-7813-455c-bc88-2eae2a91e3d5" });
        }
    }
}
