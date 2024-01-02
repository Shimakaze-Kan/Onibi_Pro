using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onibi_Pro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20240101212114 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Couriers_RegionalManagers_RegionalManagerId",
                table: "Couriers");

            migrationBuilder.DropIndex(
                name: "IX_RegionalManagers_UserId",
                table: "RegionalManagers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Couriers",
                table: "Couriers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "RegionalManagers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "RegionalManagers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Couriers");

            migrationBuilder.AlterColumn<Guid>(
                name: "RegionalManagerId",
                table: "Couriers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Couriers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Couriers",
                table: "Couriers",
                column: "CourierId");

            migrationBuilder.CreateIndex(
                name: "IX_RegionalManagers_UserId",
                table: "RegionalManagers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Couriers_UserId",
                table: "Couriers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Couriers_RegionalManagers_RegionalManagerId",
                table: "Couriers",
                column: "RegionalManagerId",
                principalTable: "RegionalManagers",
                principalColumn: "RegionalManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Couriers_Users_UserId",
                table: "Couriers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Couriers_RegionalManagers_RegionalManagerId",
                table: "Couriers");

            migrationBuilder.DropForeignKey(
                name: "FK_Couriers_Users_UserId",
                table: "Couriers");

            migrationBuilder.DropIndex(
                name: "IX_RegionalManagers_UserId",
                table: "RegionalManagers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Couriers",
                table: "Couriers");

            migrationBuilder.DropIndex(
                name: "IX_Couriers_UserId",
                table: "Couriers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Couriers");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "RegionalManagers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "RegionalManagers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "RegionalManagerId",
                table: "Couriers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Couriers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Couriers",
                table: "Couriers",
                columns: new[] { "CourierId", "RegionalManagerId" });

            migrationBuilder.CreateIndex(
                name: "IX_RegionalManagers_UserId",
                table: "RegionalManagers",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Couriers_RegionalManagers_RegionalManagerId",
                table: "Couriers",
                column: "RegionalManagerId",
                principalTable: "RegionalManagers",
                principalColumn: "RegionalManagerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
