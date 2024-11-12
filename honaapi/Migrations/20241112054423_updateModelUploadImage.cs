using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace honaapi.Migrations
{
    /// <inheritdoc />
    public partial class updateModelUploadImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UploadImages_Products_ProductId",
                table: "UploadImages");

            migrationBuilder.DropIndex(
                name: "IX_UploadImages_ProductId",
                table: "UploadImages");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "UploadImages");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UploadImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "UploadImages");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "UploadImages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UploadImages_ProductId",
                table: "UploadImages",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_UploadImages_Products_ProductId",
                table: "UploadImages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
