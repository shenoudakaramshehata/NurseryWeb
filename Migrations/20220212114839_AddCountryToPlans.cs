using Microsoft.EntityFrameworkCore.Migrations;

namespace Nursery.Migrations
{
    public partial class AddCountryToPlans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Plan_CountryId",
                table: "Plan",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plan_Country_CountryId",
                table: "Plan",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plan_Country_CountryId",
                table: "Plan");

            migrationBuilder.DropIndex(
                name: "IX_Plan_CountryId",
                table: "Plan");
        }
    }
}
