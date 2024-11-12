using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace honaapi.Migrations
{
    /// <inheritdoc />
    public partial class AddModelVariantProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "VariantProducts",
                newName: "InventoryQuantity");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "BasePrice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InventoryQuantity",
                table: "VariantProducts",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "BasePrice",
                table: "Products",
                newName: "Price");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
