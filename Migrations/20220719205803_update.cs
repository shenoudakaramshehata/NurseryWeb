using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nursery.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Auth",
                table: "NurseryMember",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "NurseryMember",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PaymentID",
                table: "NurseryMember",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PostDate",
                table: "NurseryMember",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ref",
                table: "NurseryMember",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "NurseryMember",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrackID",
                table: "NurseryMember",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TranID",
                table: "NurseryMember",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "payment_type",
                table: "NurseryMember",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Auth",
                table: "NurseryMember");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "NurseryMember");

            migrationBuilder.DropColumn(
                name: "PaymentID",
                table: "NurseryMember");

            migrationBuilder.DropColumn(
                name: "PostDate",
                table: "NurseryMember");

            migrationBuilder.DropColumn(
                name: "Ref",
                table: "NurseryMember");

            migrationBuilder.DropColumn(
                name: "Result",
                table: "NurseryMember");

            migrationBuilder.DropColumn(
                name: "TrackID",
                table: "NurseryMember");

            migrationBuilder.DropColumn(
                name: "TranID",
                table: "NurseryMember");

            migrationBuilder.DropColumn(
                name: "payment_type",
                table: "NurseryMember");
        }
    }
}
