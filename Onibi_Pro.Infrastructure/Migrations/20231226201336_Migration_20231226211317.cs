using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onibi_Pro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20231226211317 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ingredients",
                table: "MenuItems");

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MenuItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MenuItemMenuId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ingredients_MenuItems_MenuItemId_MenuItemMenuId",
                        columns: x => new { x.MenuItemId, x.MenuItemMenuId },
                        principalTable: "MenuItems",
                        principalColumns: new[] { "MenuItemId", "MenuId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_MenuItemId_MenuItemMenuId",
                table: "Ingredients",
                columns: new[] { "MenuItemId", "MenuItemMenuId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.AddColumn<string>(
                name: "Ingredients",
                table: "MenuItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
