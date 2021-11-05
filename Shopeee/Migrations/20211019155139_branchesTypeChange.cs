using Microsoft.EntityFrameworkCore.Migrations;

namespace Shopeee.Migrations
{
    public partial class branchesTypeChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "Branch",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Branch",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Area",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Branch");

        }
    }
}
