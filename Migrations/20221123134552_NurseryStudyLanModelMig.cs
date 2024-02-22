using Microsoft.EntityFrameworkCore.Migrations;

namespace Nursery.Migrations
{
    public partial class NurseryStudyLanModelMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NurseryStudyLanguages",
                columns: table => new
                {
                    NurseryStudyLanguageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NurseryId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NurseryStudyLanguages", x => x.NurseryStudyLanguageId);
                    table.ForeignKey(
                        name: "FK_NurseryStudyLanguages_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "LanguageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NurseryStudyLanguages_NurseryMember_NurseryId",
                        column: x => x.NurseryId,
                        principalTable: "NurseryMember",
                        principalColumn: "NurseryMemberId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NurseryStudyLanguages_LanguageId",
                table: "NurseryStudyLanguages",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_NurseryStudyLanguages_NurseryId",
                table: "NurseryStudyLanguages",
                column: "NurseryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NurseryStudyLanguages");
        }
    }
}
