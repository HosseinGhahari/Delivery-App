using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Delivery_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedIsRemovedFromUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }
    }
}
