using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace POEPART2CMCSFINAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
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
                table: "Users",
                columns: new[] { "ID", "Name", "password", "role", "username" },
                values: new object[,]
                {
                    { 1, "John", "password1", "Lecturer", "john@123.com" },
                    { 2, "Harry", "password2", "Coordinator", "hary@123.com" }
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Claims");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
