using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreeBirds.Migrations
{
    /// <inheritdoc />
    public partial class AddLogsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Level = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Exception = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                    Source = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Action = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    IPAddress = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 17, 7, 52, 45, 62, DateTimeKind.Utc).AddTicks(3930));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 17, 7, 52, 45, 62, DateTimeKind.Utc).AddTicks(3930));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 17, 7, 48, 24, 132, DateTimeKind.Utc).AddTicks(4520));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 17, 7, 48, 24, 132, DateTimeKind.Utc).AddTicks(4520));
        }
    }
}
