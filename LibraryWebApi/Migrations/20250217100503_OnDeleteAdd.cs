using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryWebApi.Migrations
{
    /// <inheritdoc />
    public partial class OnDeleteAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Country", "DateOfBirth", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, "Великобритания", new DateTime(1903, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Джордж", "Оруэлл" },
                    { 2, "Россия", new DateTime(1821, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Фёдор", "Достоевский" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "PasswordHash", "RefreshToken", "RefreshTokenExpiryTime", "Role" },
                values: new object[,]
                {
                    { 1, "admin@example.com", "Admin", "User", "hashed_password", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin" },
                    { 2, "user@example.com", "Regular", "User", "hashed_password", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "BorrowedAt", "Description", "Genre", "ISBN", "ReturnBy", "Title" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 2, 17, 10, 3, 57, 412, DateTimeKind.Utc).AddTicks(4518), "Культовая антиутопия о тоталитарном обществе.", "Антиутопия", "9780451524935", new DateTime(2025, 3, 3, 10, 3, 57, 412, DateTimeKind.Utc).AddTicks(4859), "1984" },
                    { 2, 2, new DateTime(2025, 2, 17, 10, 3, 57, 412, DateTimeKind.Utc).AddTicks(5234), "Философский роман о нравственности и преступлении.", "Роман", "9780140449136", new DateTime(2025, 3, 3, 10, 3, 57, 412, DateTimeKind.Utc).AddTicks(5234), "Преступление и наказание" }
                });
        }
    }
}
