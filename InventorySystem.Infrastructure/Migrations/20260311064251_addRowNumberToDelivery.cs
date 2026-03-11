using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventorySystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addRowNumberToDelivery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryLines",
                table: "DeliveryLines");

            migrationBuilder.AddColumn<int>(
                name: "RowNumber",
                table: "DeliveryLines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryLines",
                table: "DeliveryLines",
                columns: new[] { "DeliveryId", "ProductId", "RowNumber" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryLines",
                table: "DeliveryLines");

            migrationBuilder.DropColumn(
                name: "RowNumber",
                table: "DeliveryLines");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryLines",
                table: "DeliveryLines",
                columns: new[] { "DeliveryId", "ProductId" });
        }
    }
}
