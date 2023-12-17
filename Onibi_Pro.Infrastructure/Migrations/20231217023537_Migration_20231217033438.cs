using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Onibi_Pro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20231217033438 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("522f27d4-26cc-4730-a8fb-f81e3ec43de8"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("863dc18f-6554-4066-85fb-12b3ad0b4b6d"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("bbccc304-4c62-4d4c-b670-b0d6aca0a082"));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("522f27d4-26cc-4730-a8fb-f81e3ec43de8"), null, "GlobalManager", "GLOBALMANAGER" },
                    { new Guid("863dc18f-6554-4066-85fb-12b3ad0b4b6d"), null, "Manager", "MANAGER" },
                    { new Guid("bbccc304-4c62-4d4c-b670-b0d6aca0a082"), null, "RegionalManager", "REGIONALMANAGER" }
                });
        }
    }
}
