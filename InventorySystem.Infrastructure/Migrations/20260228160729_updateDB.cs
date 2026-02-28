using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventorySystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "SupplierProductPrices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.RestartSequence(
                name: "PurchaseOrderSequence",
                startValue: 10L);

            migrationBuilder.RestartSequence(
                name: "GoodsReceiptSequence",
                startValue: 10L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "SupplierProductPrices");

            migrationBuilder.RestartSequence(
                name: "PurchaseOrderSequence",
                startValue: 1L);

            migrationBuilder.RestartSequence(
                name: "GoodsReceiptSequence",
                startValue: 1L);
        }
    }
}
