using Microsoft.EntityFrameworkCore.Migrations;

namespace Nursery.Migrations
{
    public partial class _ُEditParentTB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentImage",
                table: "Parent");

            migrationBuilder.DropColumn(
                name: "ParentNameAr",
                table: "Parent");

            migrationBuilder.RenameColumn(
                name: "ParentNameEn",
                table: "Parent",
                newName: "ParentName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParentName",
                table: "Parent",
                newName: "ParentNameEn");

            migrationBuilder.AddColumn<string>(
                name: "ParentImage",
                table: "Parent",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentNameAr",
                table: "Parent",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
