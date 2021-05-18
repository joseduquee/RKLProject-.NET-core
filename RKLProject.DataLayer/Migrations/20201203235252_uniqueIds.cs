using Microsoft.EntityFrameworkCore.Migrations;

namespace RKLProject.DataLayer.Migrations
{
    public partial class uniqueIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UniqueId",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UniqueId",
                table: "Forms",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "UniqueId",
                table: "Forms");
        }
    }
}
