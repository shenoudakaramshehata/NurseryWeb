using Microsoft.EntityFrameworkCore.Migrations;

namespace Nursery.Migrations
{
    public partial class EditChildTb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Child_NurseryMember_NurseryMemberId",
                table: "Child");

            migrationBuilder.AlterColumn<int>(
                name: "NurseryMemberId",
                table: "Child",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Child_NurseryMember_NurseryMemberId",
                table: "Child",
                column: "NurseryMemberId",
                principalTable: "NurseryMember",
                principalColumn: "NurseryMemberId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Child_NurseryMember_NurseryMemberId",
                table: "Child");

            migrationBuilder.AlterColumn<int>(
                name: "NurseryMemberId",
                table: "Child",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Child_NurseryMember_NurseryMemberId",
                table: "Child",
                column: "NurseryMemberId",
                principalTable: "NurseryMember",
                principalColumn: "NurseryMemberId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
