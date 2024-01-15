using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onibi_Pro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20240114163056 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "MenuItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MenuItems");
        }
    }
}
