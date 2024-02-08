using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleMarketplaceApp.Migrations
{
    /// <inheritdoc />
    public partial class AddImageURLToItemImageModelTakeTwo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageURL",
                table: "ItemImages",
                newName: "ImageMimeType");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "ItemImages",
                type: "longblob",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "ItemImages");

            migrationBuilder.RenameColumn(
                name: "ImageMimeType",
                table: "ItemImages",
                newName: "ImageURL");
        }
    }
}
