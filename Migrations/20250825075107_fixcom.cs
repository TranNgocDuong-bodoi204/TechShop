using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnicalShop.Migrations
{
    /// <inheritdoc />
    public partial class fixcom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userComment",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userComment",
                table: "Comments");
        }
    }
}
