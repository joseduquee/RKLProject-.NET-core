using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RKLProject.DataLayer.Migrations
{
    public partial class addFormForMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterForms",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    FormName = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterForms_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MasterFormDetails",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    DisplayName = table.Column<string>(nullable: true),
                    DisplayOrder = table.Column<int>(nullable: false),
                    Required = table.Column<bool>(nullable: false),
                    ElementId = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    ReadOnly = table.Column<bool>(nullable: false),
                    FormId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterFormDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterFormDetails_MasterForms_FormId",
                        column: x => x.FormId,
                        principalTable: "MasterForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterFormDetails_FormId",
                table: "MasterFormDetails",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterForms_UserId",
                table: "MasterForms",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterFormDetails");

            migrationBuilder.DropTable(
                name: "MasterForms");
        }
    }
}
