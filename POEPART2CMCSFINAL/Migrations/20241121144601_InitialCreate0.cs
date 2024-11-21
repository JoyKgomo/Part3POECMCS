using System;
using Microsoft.EntityFrameworkCore.Migrations;
using POEPART2CMCSFINAL.Models;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace POEPART2CMCSFINAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Document",
                table: "Claims");

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClaimId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateUploaded = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateClaimed",
                value: new DateTime(2024, 11, 21, 16, 46, 0, 909, DateTimeKind.Local).AddTicks(9609));

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateClaimed",
                value: new DateTime(2024, 11, 21, 16, 46, 0, 909, DateTimeKind.Local).AddTicks(9622));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 2,
                column: "role",
                value: "Programme Coordinator");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "Name", "password", "role", "username" },
                values: new object[,]
                {
                    { 3, "Joy", "joykgomo", "HR", "joy@kg.com" },
                    { 4, "Sam", "password3", "Academic Manager", "sam@123.com" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.AddColumn<string>(
                name: "Document",
                table: "Claims",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateClaimed", "Document" },
                values: new object[] { new DateTime(2024, 10, 28, 12, 38, 29, 432, DateTimeKind.Local).AddTicks(1610), "" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateClaimed", "Document" },
                values: new object[] { new DateTime(2024, 10, 28, 12, 38, 29, 432, DateTimeKind.Local).AddTicks(1629), "" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 2,
                column: "role",
                value: "Coordinator");

            migrationBuilder.DropTable(
       name: "Claims");

            // Now drop the Users table
            migrationBuilder.DropTable(
                name: "Users");

        }
    }
}
