using Microsoft.EntityFrameworkCore.Migrations;

namespace Shopeee.Migrations
{
    public partial class resetPermissions3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermissionId",
                table: "User");

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    Privilege = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Permissions_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.AddColumn<int>(
                name: "PermissionId",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
