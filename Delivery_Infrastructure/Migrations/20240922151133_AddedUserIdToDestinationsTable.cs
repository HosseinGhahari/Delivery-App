using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Delivery_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserIdToDestinationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Destination",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Destination");
        }
    }
}
