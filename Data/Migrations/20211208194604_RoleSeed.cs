using Microsoft.EntityFrameworkCore.Migrations;

namespace Nursery.Data.Migrations
{
    public partial class RoleSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a301e986-6a05-4889-80fb-4baaa314a9c7", "3e391e2a-d935-4ce7-9a84-67ec27597ffb", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7c6e5f3e-e3cb-4978-88c1-38144a2034b9", "e83a487a-e4e6-4cc7-b49e-49da1662d474", "Customer", "CUSTOMER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7c6e5f3e-e3cb-4978-88c1-38144a2034b9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a301e986-6a05-4889-80fb-4baaa314a9c7");
        }
    }
}
