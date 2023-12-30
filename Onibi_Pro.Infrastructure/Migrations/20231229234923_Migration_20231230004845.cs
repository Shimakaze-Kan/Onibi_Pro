using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onibi_Pro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20231230004845 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Couriers_Shipment_ShipmentId",
                table: "Couriers");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Shipment_ShipmentId",
                table: "Packages");

            migrationBuilder.DropTable(
                name: "Shipment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Packages",
                table: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_Packages_ShipmentId",
                table: "Packages");

            migrationBuilder.RenameColumn(
                name: "RestaurantSourceManager",
                table: "Packages",
                newName: "SourceRestaurant");

            migrationBuilder.RenameColumn(
                name: "ShipmentId",
                table: "Packages",
                newName: "DestinationRestaurant");

            migrationBuilder.RenameColumn(
                name: "ShipmentId",
                table: "Couriers",
                newName: "RegionalManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Couriers_ShipmentId",
                table: "Couriers",
                newName: "IX_Couriers_RegionalManagerId");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Couriers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Couriers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Packages",
                table: "Packages",
                column: "PackageId");

            migrationBuilder.CreateTable(
                name: "RegionalManagers",
                columns: table => new
                {
                    RegionalManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionalManagers", x => x.RegionalManagerId);
                    table.ForeignKey(
                        name: "FK_RegionalManagers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegionalManagerRestaurantIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegionalManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionalManagerRestaurantIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegionalManagerRestaurantIds_RegionalManagers_RegionalManagerId",
                        column: x => x.RegionalManagerId,
                        principalTable: "RegionalManagers",
                        principalColumn: "RegionalManagerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Packages_DestinationRestaurant",
                table: "Packages",
                column: "DestinationRestaurant",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Packages_RegionalManager",
                table: "Packages",
                column: "RegionalManager",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegionalManagerRestaurantIds_RegionalManagerId",
                table: "RegionalManagerRestaurantIds",
                column: "RegionalManagerId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_RegionalManagers_RegionalManager",
                table: "Packages",
                column: "RegionalManager",
                principalTable: "RegionalManagers",
                principalColumn: "RegionalManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Restaurants_DestinationRestaurant",
                table: "Packages",
                column: "DestinationRestaurant",
                principalTable: "Restaurants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Couriers_RegionalManagers_RegionalManagerId",
                table: "Couriers");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_RegionalManagers_RegionalManager",
                table: "Packages");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Restaurants_DestinationRestaurant",
                table: "Packages");

            migrationBuilder.DropTable(
                name: "RegionalManagerRestaurantIds");

            migrationBuilder.DropTable(
                name: "RegionalManagers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Packages",
                table: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_Packages_DestinationRestaurant",
                table: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_Packages_RegionalManager",
                table: "Packages");

            migrationBuilder.RenameColumn(
                name: "SourceRestaurant",
                table: "Packages",
                newName: "RestaurantSourceManager");

            migrationBuilder.RenameColumn(
                name: "DestinationRestaurant",
                table: "Packages",
                newName: "ShipmentId");

            migrationBuilder.RenameColumn(
                name: "RegionalManagerId",
                table: "Couriers",
                newName: "ShipmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Couriers_RegionalManagerId",
                table: "Couriers",
                newName: "IX_Couriers_ShipmentId");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Couriers",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Couriers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Packages",
                table: "Packages",
                columns: new[] { "PackageId", "ShipmentId" });

            migrationBuilder.CreateTable(
                name: "Shipment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipment", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Packages_ShipmentId",
                table: "Packages",
                column: "ShipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Couriers_Shipment_ShipmentId",
                table: "Couriers",
                column: "ShipmentId",
                principalTable: "Shipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Shipment_ShipmentId",
                table: "Packages",
                column: "ShipmentId",
                principalTable: "Shipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
