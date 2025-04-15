using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreeBirds.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePasswordToHashed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "Password",
                value: "$2a$11$IA1SWZw/3/ly37z/o3wjze8MDNnse5ND6TcKHkixOmfqR9csa7rrO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "Password",
                value: "testpassword");
        }
    }
}
