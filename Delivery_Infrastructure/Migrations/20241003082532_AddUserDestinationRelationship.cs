using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Delivery_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserDestinationRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Delivery_AspNetUsers_UserId",
                table: "Delivery");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Destination",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Destination_UserId",
                table: "Destination",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Delivery_AspNetUsers_UserId",
                table: "Delivery",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Destination_AspNetUsers_UserId",
                table: "Destination",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Delivery_AspNetUsers_UserId",
                table: "Delivery");

            migrationBuilder.DropForeignKey(
                name: "FK_Destination_AspNetUsers_UserId",
                table: "Destination");

            migrationBuilder.DropIndex(
                name: "IX_Destination_UserId",
                table: "Destination");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Destination",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Delivery_AspNetUsers_UserId",
                table: "Delivery",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
