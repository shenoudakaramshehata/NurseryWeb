using Microsoft.EntityFrameworkCore.Migrations;

namespace Nursery.Migrations
{
    public partial class MacM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MacsAccounts",
                columns: table => new
                {
                    MacsAccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MacDevice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NurseryMemberId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MacsAccounts", x => x.MacsAccountId);
                    table.ForeignKey(
                        name: "FK_MacsAccounts_NurseryMember_NurseryMemberId",
                        column: x => x.NurseryMemberId,
                        principalTable: "NurseryMember",
                        principalColumn: "NurseryMemberId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MacsAccounts_NurseryMemberId",
                table: "MacsAccounts",
                column: "NurseryMemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MacsAccounts");
        }
    }
}
