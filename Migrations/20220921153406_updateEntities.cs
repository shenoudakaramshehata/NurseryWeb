using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nursery.Migrations
{
    public partial class updateEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NurseryMember_PaymentMethod_PaymentMethodId",
                table: "NurseryMember");

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

            migrationBuilder.AddColumn<string>(
                name: "Auth",
                table: "NurserySubscription",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "NurserySubscription",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PaymentID",
                table: "NurserySubscription",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                table: "NurserySubscription",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlanTypeId",
                table: "NurserySubscription",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PostDate",
                table: "NurserySubscription",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ref",
                table: "NurserySubscription",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "NurserySubscription",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrackID",
                table: "NurserySubscription",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TranID",
                table: "NurserySubscription",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "payment_type",
                table: "NurserySubscription",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PaymentMethodId",
                table: "NurseryMember",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_NurserySubscription_PaymentMethodId",
                table: "NurserySubscription",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_NurseryMember_PaymentMethod_PaymentMethodId",
                table: "NurseryMember",
                column: "PaymentMethodId",
                principalTable: "PaymentMethod",
                principalColumn: "PaymentMethodId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NurserySubscription_PaymentMethod_PaymentMethodId",
                table: "NurserySubscription",
                column: "PaymentMethodId",
                principalTable: "PaymentMethod",
                principalColumn: "PaymentMethodId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NurseryMember_PaymentMethod_PaymentMethodId",
                table: "NurseryMember");

            migrationBuilder.DropForeignKey(
                name: "FK_NurserySubscription_PaymentMethod_PaymentMethodId",
                table: "NurserySubscription");

            migrationBuilder.DropIndex(
                name: "IX_NurserySubscription_PaymentMethodId",
                table: "NurserySubscription");

            migrationBuilder.DropColumn(
                name: "Auth",
                table: "NurserySubscription");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "NurserySubscription");

            migrationBuilder.DropColumn(
                name: "PaymentID",
                table: "NurserySubscription");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "NurserySubscription");

            migrationBuilder.DropColumn(
                name: "PlanTypeId",
                table: "NurserySubscription");

            migrationBuilder.DropColumn(
                name: "PostDate",
                table: "NurserySubscription");

            migrationBuilder.DropColumn(
                name: "Ref",
                table: "NurserySubscription");

            migrationBuilder.DropColumn(
                name: "Result",
                table: "NurserySubscription");

            migrationBuilder.DropColumn(
                name: "TrackID",
                table: "NurserySubscription");

            migrationBuilder.DropColumn(
                name: "TranID",
                table: "NurserySubscription");

            migrationBuilder.DropColumn(
                name: "payment_type",
                table: "NurserySubscription");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentMethodId",
                table: "NurseryMember",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_NurseryMember_PaymentMethod_PaymentMethodId",
                table: "NurseryMember",
                column: "PaymentMethodId",
                principalTable: "PaymentMethod",
                principalColumn: "PaymentMethodId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
