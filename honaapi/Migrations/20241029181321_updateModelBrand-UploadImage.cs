using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace honaapi.Migrations
{
    /// <inheritdoc />
    public partial class updateModelBrandUploadImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UploadImages_Brands_BrandId",
                table: "UploadImages");

            migrationBuilder.DropIndex(
                name: "IX_UploadImages_BrandId",
                table: "UploadImages");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "UploadImages");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Brands");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "UploadImages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UploadImages_BrandId",
                table: "UploadImages",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_UploadImages_Brands_BrandId",
                table: "UploadImages",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");
        }
    }
}
