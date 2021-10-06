using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication8.Migrations
{
    public partial class _1501 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Relatives_AspNetUsers_ApplicationUserId",
                table: "Relatives");

            migrationBuilder.DropIndex(
                name: "IX_Relatives_ApplicationUserId",
                table: "Relatives");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Relatives");

            migrationBuilder.DropColumn(
                name: "GroupCode",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "GroupCode",
                table: "Relatives",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "RelativesApplicationUsers",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    GroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelativesApplicationUsers", x => new { x.GroupId, x.UserId });
                    table.ForeignKey(
                        name: "FK_RelativesApplicationUsers_Relatives_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Relatives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RelativesApplicationUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RelativesApplicationUsers_UserId",
                table: "RelativesApplicationUsers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RelativesApplicationUsers");

            migrationBuilder.AlterColumn<string>(
                name: "GroupCode",
                table: "Relatives",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Relatives",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupCode",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Relatives_ApplicationUserId",
                table: "Relatives",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Relatives_AspNetUsers_ApplicationUserId",
                table: "Relatives",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
