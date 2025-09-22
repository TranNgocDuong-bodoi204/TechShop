using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnicalShop.Migrations
{
    /// <inheritdoc />
    public partial class orderstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "orderStatus",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "paymentStatus",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "orderStatus",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "paymentStatus",
                table: "Orders");
        }
    }
}
