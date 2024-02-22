using Microsoft.EntityFrameworkCore.Migrations;

namespace Nursery.Migrations
{
    public partial class AddSliderToNursery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSlider",
                table: "NurseryMember",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSlider",
                table: "NurseryMember");
        }
    }
}
