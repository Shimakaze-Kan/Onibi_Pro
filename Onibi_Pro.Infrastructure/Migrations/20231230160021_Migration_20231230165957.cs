using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onibi_Pro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20231230165957 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Packages_RegionalManager",
                table: "Packages");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_RegionalManager",
                table: "Packages",
                column: "RegionalManager");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Packages_RegionalManager",
                table: "Packages");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_RegionalManager",
                table: "Packages",
                column: "RegionalManager",
                unique: true);
        }
    }
}
