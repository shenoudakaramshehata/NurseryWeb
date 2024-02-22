using Microsoft.EntityFrameworkCore.Migrations;

namespace Nursery.Migrations
{
    public partial class seddingPlanType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlanTypes",
                columns: table => new
                {
                    PlanTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cost = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanTypes", x => x.PlanTypeId);
                });

            migrationBuilder.InsertData(
                table: "PlanTypes",
                columns: new[] { "PlanTypeId", "Cost", "Title" },
                values: new object[] { 1, 0.0, "Silver" });

            migrationBuilder.InsertData(
                table: "PlanTypes",
                columns: new[] { "PlanTypeId", "Cost", "Title" },
                values: new object[] { 2, 5.0, "Gold" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanTypes");
        }
    }
}
