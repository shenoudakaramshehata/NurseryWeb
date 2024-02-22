using Microsoft.EntityFrameworkCore.Migrations;

namespace Nursery.Migrations
{
    public partial class UpdateNurModelMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "NurseryMember",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudyLanguage",
                table: "NurseryMember",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "NurseryMember");

            migrationBuilder.DropColumn(
                name: "StudyLanguage",
                table: "NurseryMember");
        }
    }
}
