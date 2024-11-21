using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POEPART2CMCSFINAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    username = table.Column<string>(type: "TEXT", nullable: false),
                    password = table.Column<string>(type: "TEXT", nullable: false),
                    role = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });
            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserID = table.Column<int>(type: "INTEGER", nullable: false),
                    status = table.Column<string>(type: "TEXT", nullable: false),
                    DateClaimed = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HoursWorked = table.Column<int>(type: "INTEGER", nullable: false),
                    HourlyRate = table.Column<double>(type: "REAL", nullable: false),
                    AmountDue = table.Column<double>(type: "REAL", nullable: false),
                    Document = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Claims_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "ID", "Name", "password", "role", "username" },
                values: new object[,]
                {
                    { 1, "John", "password1", "Lecturer", "john@123.com" },
                    { 2, "Harry", "password2", "Coordinator", "hary@123.com" },
                    { 3, "Joy", "joykgomo", "HR", "joy@kg.com" },
                    { 4, "Sam", "password3", "Academic Manager", "sam@123.com" }
                });

            migrationBuilder.InsertData(
               table: "Claims",
               columns: new[] { "Id", "AmountDue", "DateClaimed", "Document", "HourlyRate", "HoursWorked", "UserID", "status" },
               values: new object[,]
               {
                    { 1, 2400.0, new DateTime(2024, 10, 28, 12, 38, 29, 432, DateTimeKind.Local).AddTicks(1610), "", 2000.0, 34, 1, "Pending" },
                    { 2, 2400.0, new DateTime(2024, 10, 28, 12, 38, 29, 432, DateTimeKind.Local).AddTicks(1629), "", 2000.0, 34, 2, "pending" }
               });

            migrationBuilder.CreateIndex(
                name: "IX_Claims_UserID",
                table: "Claims",
                column: "UserID");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateClaimed",
                value: new DateTime(2024, 11, 21, 16, 57, 57, 162, DateTimeKind.Local).AddTicks(1281));

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateClaimed",
                value: new DateTime(2024, 11, 21, 16, 57, 57, 162, DateTimeKind.Local).AddTicks(1297));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
