using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreApiDirect.Demo.Migrations
{
    public partial class AddNotesAtEntityBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Lessons",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Lessons");
        }
    }
}
