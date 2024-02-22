using Microsoft.EntityFrameworkCore.Migrations;

namespace Nursery.Migrations
{
    public partial class AddNotifyType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntityTypeNotifies",
                columns: table => new
                {
                    EntityTypeNotifyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityTypeNotifies", x => x.EntityTypeNotifyId);
                });

            migrationBuilder.InsertData(
                table: "EntityTypeNotifies",
                columns: new[] { "EntityTypeNotifyId", "TitleAr", "TitleEn" },
                values: new object[] { 1, "حضانة", "Nursery" });

            migrationBuilder.InsertData(
                table: "EntityTypeNotifies",
                columns: new[] { "EntityTypeNotifyId", "TitleAr", "TitleEn" },
                values: new object[] { 2, "اخري", "Others" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityTypeNotifies");
        }
    }
}
