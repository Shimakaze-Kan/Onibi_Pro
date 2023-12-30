using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onibi_Pro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20231230165541 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Packages_DestinationRestaurant",
                table: "Packages");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_DestinationRestaurant",
                table: "Packages",
                column: "DestinationRestaurant");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Packages_DestinationRestaurant",
                table: "Packages");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_DestinationRestaurant",
                table: "Packages",
                column: "DestinationRestaurant",
                unique: true);
        }
    }
}
