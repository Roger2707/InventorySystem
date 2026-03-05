using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventorySystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addFiledInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SourceReceiptLineId",
                table: "InventoryCostLayers",
                newName: "GoodsReceiptId");

            migrationBuilder.AddColumn<decimal>(
                name: "UnitCost",
                table: "InventoryLedgers",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OriginalQty",
                table: "InventoryCostLayers",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceiptDate",
                table: "InventoryCostLayers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitCost",
                table: "InventoryLedgers");

            migrationBuilder.DropColumn(
                name: "OriginalQty",
                table: "InventoryCostLayers");

            migrationBuilder.DropColumn(
                name: "ReceiptDate",
                table: "InventoryCostLayers");

            migrationBuilder.RenameColumn(
                name: "GoodsReceiptId",
                table: "InventoryCostLayers",
                newName: "SourceReceiptLineId");
        }
    }
}
