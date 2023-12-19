using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onibi_Pro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20231219001437 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Managers");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Managers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Managers_UserId",
                table: "Managers",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_Users_UserId",
                table: "Managers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Managers_Users_UserId",
                table: "Managers");

            migrationBuilder.DropIndex(
                name: "IX_Managers_UserId",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Managers");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Managers",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Managers",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Managers",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");
        }
    }
}
