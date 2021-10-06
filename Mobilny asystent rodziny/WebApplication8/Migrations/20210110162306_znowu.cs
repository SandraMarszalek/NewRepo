using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication8.Migrations
{
    public partial class znowu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskGroups_TasksGroups",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_TasksGroups",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TasksGroups",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "TaskGroups",
                table: "Tasks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskGroups",
                table: "Tasks",
                column: "TaskGroups");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskGroups_TaskGroups",
                table: "Tasks",
                column: "TaskGroups",
                principalTable: "TaskGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskGroups_TaskGroups",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_TaskGroups",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskGroups",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "TasksGroups",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TasksGroups",
                table: "Tasks",
                column: "TasksGroups");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskGroups_TasksGroups",
                table: "Tasks",
                column: "TasksGroups",
                principalTable: "TaskGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
