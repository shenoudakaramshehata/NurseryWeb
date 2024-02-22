﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Nursery.Migrations
{
    public partial class updateNMMig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "NurseryMember",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "NurseryMember");
        }
    }
}
