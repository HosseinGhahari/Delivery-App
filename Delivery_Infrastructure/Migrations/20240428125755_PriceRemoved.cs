using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Delivery_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PriceRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Delivery");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Delivery",
                type: "float",
                nullable: true);
        }
    }
}
