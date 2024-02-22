using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nursery.Migrations
{
    public partial class AddnewsLetterAndEditChidRerlation : Migration
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
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Newsletter",
                columns: table => new
                {
                    NewsletterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Newsletter", x => x.NewsletterId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Child_NurseryMember_NurseryMemberId",
                table: "Child",
                column: "NurseryMemberId",
                principalTable: "NurseryMember",
                principalColumn: "NurseryMemberId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Child_NurseryMember_NurseryMemberId",
                table: "Child");

            migrationBuilder.DropTable(
                name: "Newsletter");

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
    }
}
