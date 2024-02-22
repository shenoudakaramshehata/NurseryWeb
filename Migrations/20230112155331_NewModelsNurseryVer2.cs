using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nursery.Migrations
{
    public partial class NewModelsNurseryVer2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcademyYears",
                columns: table => new
                {
                    AcademyYearId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    NurseryMemberId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademyYears", x => x.AcademyYearId);
                    table.ForeignKey(
                        name: "FK_AcademyYears_NurseryMember_NurseryMemberId",
                        column: x => x.NurseryMemberId,
                        principalTable: "NurseryMember",
                        principalColumn: "NurseryMemberId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentTypeAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentTypeEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.DocumentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NurseryMemberId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_Groups_NurseryMember_NurseryMemberId",
                        column: x => x.NurseryMemberId,
                        principalTable: "NurseryMember",
                        principalColumn: "NurseryMemberId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NurseryPlanTypes",
                columns: table => new
                {
                    NurseryPlanTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NurseryPlanTypeTitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NurseryPlanTypeTitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NurseryPlanTypes", x => x.NurseryPlanTypeId);
                });

            migrationBuilder.CreateTable(
                name: "NurseryPlans",
                columns: table => new
                {
                    NurseryPlanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanTitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanTitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    NurseryPlanTypeId = table.Column<int>(type: "int", nullable: false),
                    NurseryMemberId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NurseryPlans", x => x.NurseryPlanId);
                    table.ForeignKey(
                        name: "FK_NurseryPlans_NurseryMember_NurseryMemberId",
                        column: x => x.NurseryMemberId,
                        principalTable: "NurseryMember",
                        principalColumn: "NurseryMemberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NurseryPlans_NurseryPlanTypes_NurseryPlanTypeId",
                        column: x => x.NurseryPlanTypeId,
                        principalTable: "NurseryPlanTypes",
                        principalColumn: "NurseryPlanTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NurseryPlanId = table.Column<int>(type: "int", nullable: true),
                    NurseryMemberId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_Students_NurseryMember_NurseryMemberId",
                        column: x => x.NurseryMemberId,
                        principalTable: "NurseryMember",
                        principalColumn: "NurseryMemberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_NurseryPlans_NurseryPlanId",
                        column: x => x.NurseryPlanId,
                        principalTable: "NurseryPlans",
                        principalColumn: "NurseryPlanId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentAttachments",
                columns: table => new
                {
                    StudentAttachmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAttachments", x => x.StudentAttachmentId);
                    table.ForeignKey(
                        name: "FK_StudentAttachments_DocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "DocumentTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAttachments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentGroups",
                columns: table => new
                {
                    StudentGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    GroupId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentGroups", x => x.StudentGroupId);
                    table.ForeignKey(
                        name: "FK_StudentGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentGroups_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademyYears_NurseryMemberId",
                table: "AcademyYears",
                column: "NurseryMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_NurseryMemberId",
                table: "Groups",
                column: "NurseryMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_NurseryPlans_NurseryMemberId",
                table: "NurseryPlans",
                column: "NurseryMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_NurseryPlans_NurseryPlanTypeId",
                table: "NurseryPlans",
                column: "NurseryPlanTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttachments_DocumentTypeId",
                table: "StudentAttachments",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttachments_StudentId",
                table: "StudentAttachments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroups_GroupId",
                table: "StudentGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroups_StudentId",
                table: "StudentGroups",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_NurseryMemberId",
                table: "Students",
                column: "NurseryMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_NurseryPlanId",
                table: "Students",
                column: "NurseryPlanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademyYears");

            migrationBuilder.DropTable(
                name: "StudentAttachments");

            migrationBuilder.DropTable(
                name: "StudentGroups");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "NurseryPlans");

            migrationBuilder.DropTable(
                name: "NurseryPlanTypes");
        }
    }
}
