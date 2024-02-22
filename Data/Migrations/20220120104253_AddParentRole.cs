using Microsoft.EntityFrameworkCore.Migrations;

namespace Nursery.Data.Migrations
{
    public partial class AddParentRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7c57e7d1-09f1-4315-981a-a7fbe8aad2c9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ca59525a-d65b-4428-a6bc-748c2645432e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ec045312-62c7-4873-83ec-357c9e517ca3", "ef5db098-c9be-4500-bb13-cf1ba62c8163", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "23657199-96ac-4386-ab20-3efa75e6dc5c", "ebc36d83-f942-4437-bce8-6c4c6dbb2851", "Nursery", "NURSERY" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c9605fd0-05a2-4dbd-8e7b-adf9edaed598", "174c37d4-e8c5-4af9-a98e-4bc9febbcae1", "Parent", "PARENT" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "23657199-96ac-4386-ab20-3efa75e6dc5c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c9605fd0-05a2-4dbd-8e7b-adf9edaed598");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ec045312-62c7-4873-83ec-357c9e517ca3");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ca59525a-d65b-4428-a6bc-748c2645432e", "b72f9fd8-729f-4af9-9175-3aee2d66663c", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7c57e7d1-09f1-4315-981a-a7fbe8aad2c9", "8c868b8f-7f3b-4c07-8f3e-bf441ee6ab8f", "Trainer", "TRAINER" });
        }
    }
}
