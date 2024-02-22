using Microsoft.EntityFrameworkCore.Migrations;

namespace Nursery.Migrations
{
    public partial class updatePubNot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PublicNotification_EntityType_EntityTypeId",
                table: "PublicNotification");

            migrationBuilder.RenameColumn(
                name: "EntityTypeId",
                table: "PublicNotification",
                newName: "EntityTypeNotifyId");

            migrationBuilder.RenameIndex(
                name: "IX_PublicNotification_EntityTypeId",
                table: "PublicNotification",
                newName: "IX_PublicNotification_EntityTypeNotifyId");

            migrationBuilder.UpdateData(
                table: "EntityTypeNotifies",
                keyColumn: "EntityTypeNotifyId",
                keyValue: 2,
                columns: new[] { "TitleAr", "TitleEn" },
                values: new object[] { "عام", "Public" });

            migrationBuilder.AddForeignKey(
                name: "FK_PublicNotification_EntityTypeNotifies_EntityTypeNotifyId",
                table: "PublicNotification",
                column: "EntityTypeNotifyId",
                principalTable: "EntityTypeNotifies",
                principalColumn: "EntityTypeNotifyId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PublicNotification_EntityTypeNotifies_EntityTypeNotifyId",
                table: "PublicNotification");

            migrationBuilder.RenameColumn(
                name: "EntityTypeNotifyId",
                table: "PublicNotification",
                newName: "EntityTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_PublicNotification_EntityTypeNotifyId",
                table: "PublicNotification",
                newName: "IX_PublicNotification_EntityTypeId");

            migrationBuilder.UpdateData(
                table: "EntityTypeNotifies",
                keyColumn: "EntityTypeNotifyId",
                keyValue: 2,
                columns: new[] { "TitleAr", "TitleEn" },
                values: new object[] { "اخري", "Others" });

            migrationBuilder.AddForeignKey(
                name: "FK_PublicNotification_EntityType_EntityTypeId",
                table: "PublicNotification",
                column: "EntityTypeId",
                principalTable: "EntityType",
                principalColumn: "EntityTypeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
