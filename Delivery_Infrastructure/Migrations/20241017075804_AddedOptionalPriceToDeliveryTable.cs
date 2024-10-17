using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Delivery_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedOptionalPriceToDeliveryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "OptionalPrice",
                table: "Delivery",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OptionalPrice",
                table: "Delivery");
        }
    }
}
