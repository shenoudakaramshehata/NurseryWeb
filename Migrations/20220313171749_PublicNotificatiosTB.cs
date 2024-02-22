using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nursery.Migrations
{
    public partial class PublicNotificatiosTB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PublicDevice",
                columns: table => new
                {
                    PublicDeviceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAndroiodDevice = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicDevice", x => x.PublicDeviceId);
                    table.ForeignKey(
                        name: "FK_PublicDevice_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PublicNotification",
                columns: table => new
                {
                    PublicNotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntityTypeId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicNotification", x => x.PublicNotificationId);
                    table.ForeignKey(
                        name: "FK_PublicNotification_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PublicNotification_EntityType_EntityTypeId",
                        column: x => x.EntityTypeId,
                        principalTable: "EntityType",
                        principalColumn: "EntityTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PublicNotificationDevice",
                columns: table => new
                {
                    PublicNotificationDeviceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicNotificationId = table.Column<int>(type: "int", nullable: false),
                    PublicDeviceId = table.Column<int>(type: "int", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicNotificationDevice", x => x.PublicNotificationDeviceId);
                    table.ForeignKey(
                        name: "FK_PublicNotificationDevice_PublicDevice_PublicDeviceId",
                        column: x => x.PublicDeviceId,
                        principalTable: "PublicDevice",
                        principalColumn: "PublicDeviceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PublicNotificationDevice_PublicNotification_PublicNotificationId",
                        column: x => x.PublicNotificationId,
                        principalTable: "PublicNotification",
                        principalColumn: "PublicNotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PublicDevice_CountryId",
                table: "PublicDevice",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicNotification_CountryId",
                table: "PublicNotification",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicNotification_EntityTypeId",
                table: "PublicNotification",
                column: "EntityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicNotificationDevice_PublicDeviceId",
                table: "PublicNotificationDevice",
                column: "PublicDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicNotificationDevice_PublicNotificationId",
                table: "PublicNotificationDevice",
                column: "PublicNotificationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublicNotificationDevice");

            migrationBuilder.DropTable(
                name: "PublicDevice");

            migrationBuilder.DropTable(
                name: "PublicNotification");
        }
    }
}
