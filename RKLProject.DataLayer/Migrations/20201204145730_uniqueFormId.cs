using Microsoft.EntityFrameworkCore.Migrations;

namespace RKLProject.DataLayer.Migrations
{
    public partial class uniqueFormId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UniqueId",
                table: "MasterForms",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueId",
                table: "MasterForms");
        }
    }
}
