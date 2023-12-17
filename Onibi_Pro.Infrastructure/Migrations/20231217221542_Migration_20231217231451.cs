using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Onibi_Pro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20231217231451 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0a94f29f-a993-4112-8122-7b8fdb5ca7c5"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0b421e97-f806-42ba-b9f7-a425febfb116"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("77b7a579-55fb-4ab5-805a-739b81e2dd10"));

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserType",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("05ae1667-9f48-4331-ba60-7d1dd42f1706"), null, "GlobalManager", "GLOBALMANAGER" },
                    { new Guid("42856a3f-62b8-4fd6-9c17-9f4adcd83edc"), null, "RegionalManager", "REGIONALMANAGER" },
                    { new Guid("f93a147f-cece-4049-8914-608824d95a34"), null, "Manager", "MANAGER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("05ae1667-9f48-4331-ba60-7d1dd42f1706"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("42856a3f-62b8-4fd6-9c17-9f4adcd83edc"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f93a147f-cece-4049-8914-608824d95a34"));

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("0a94f29f-a993-4112-8122-7b8fdb5ca7c5"), null, "Manager", "MANAGER" },
                    { new Guid("0b421e97-f806-42ba-b9f7-a425febfb116"), null, "RegionalManager", "REGIONALMANAGER" },
                    { new Guid("77b7a579-55fb-4ab5-805a-739b81e2dd10"), null, "GlobalManager", "GLOBALMANAGER" }
                });
        }
    }
}
