using Microsoft.EntityFrameworkCore.Migrations;

namespace Shopeee.Migrations
{
    public partial class NewShoppingCartModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "ShoppingCart");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "TotalPrice",
                table: "ShoppingCart",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
