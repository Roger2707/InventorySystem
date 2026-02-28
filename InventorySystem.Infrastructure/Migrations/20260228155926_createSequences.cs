using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventorySystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class createSequences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "GoodsReceiptSequence");

            migrationBuilder.CreateSequence<int>(
                name: "PurchaseOrderSequence");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "GoodsReceiptSequence");

            migrationBuilder.DropSequence(
                name: "PurchaseOrderSequence");
        }
    }
}
