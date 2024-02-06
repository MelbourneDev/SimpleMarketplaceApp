using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleMarketplaceApp.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPastListingToItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPastListing",
                table: "Items",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPastListing",
                table: "Items");
        }
    }
}
