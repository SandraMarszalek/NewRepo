using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication8.Migrations
{
    public partial class userlocationonmap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userId",
                table: "Map");

            migrationBuilder.CreateTable(
                name: "UsersMapLocations",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    MapId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersMapLocations", x => new { x.MapId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UsersMapLocations_Map_MapId",
                        column: x => x.MapId,
                        principalTable: "Map",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersMapLocations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersMapLocations_UserId",
                table: "UsersMapLocations",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersMapLocations");

            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "Map",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
