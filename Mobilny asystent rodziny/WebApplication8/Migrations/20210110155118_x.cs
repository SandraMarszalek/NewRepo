using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication8.Migrations
{
    public partial class x : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskGroups_TaskGroupsId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_TaskGroupsId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskGroupsId",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "TasksGroups",
                table: "Tasks",
                nullable: true);

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
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "TaskGroupsId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskGroupsId",
                table: "Tasks",
                column: "TaskGroupsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskGroups_TaskGroupsId",
                table: "Tasks",
                column: "TaskGroupsId",
                principalTable: "TaskGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
