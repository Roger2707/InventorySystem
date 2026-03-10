using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventorySystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addRowNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesOrderLines",
                table: "SalesOrderLines");

            migrationBuilder.AddColumn<int>(
                name: "RowNumber",
                table: "SalesOrderLines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesOrderLines",
                table: "SalesOrderLines",
                columns: new[] { "SalesOrderId", "ProductId", "RowNumber" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesOrderLines",
                table: "SalesOrderLines");

            migrationBuilder.DropColumn(
                name: "RowNumber",
                table: "SalesOrderLines");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesOrderLines",
                table: "SalesOrderLines",
                columns: new[] { "SalesOrderId", "ProductId" });
        }
    }
}
