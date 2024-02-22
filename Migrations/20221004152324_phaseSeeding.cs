using Microsoft.EntityFrameworkCore.Migrations;

namespace Nursery.Migrations
{
    public partial class phaseSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Phases",
                columns: new[] { "PhaseId", "TitleAR", "TitleEN" },
                values: new object[,]
                {
                    { 1, "يوم", "Day" },
                    { 2, "أسبوع", "Week" },
                    { 3, "شهر", "Month" },
                    { 4, "سنة", "Year" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Phases",
                keyColumn: "PhaseId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Phases",
                keyColumn: "PhaseId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Phases",
                keyColumn: "PhaseId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Phases",
                keyColumn: "PhaseId",
                keyValue: 4);
        }
    }
}
