using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nursery.Migrations
{
    public partial class update_child : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Child");

            migrationBuilder.AddColumn<int>(
                name: "AgeCategoryId",
                table: "Child",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Child_AgeCategoryId",
                table: "Child",
                column: "AgeCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Child_AgeCategory_AgeCategoryId",
                table: "Child",
                column: "AgeCategoryId",
                principalTable: "AgeCategory",
                principalColumn: "AgeCategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Child_AgeCategory_AgeCategoryId",
                table: "Child");

            migrationBuilder.DropIndex(
                name: "IX_Child_AgeCategoryId",
                table: "Child");

            migrationBuilder.DropColumn(
                name: "AgeCategoryId",
                table: "Child");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Child",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
