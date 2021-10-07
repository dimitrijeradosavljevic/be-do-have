using Microsoft.EntityFrameworkCore.Migrations;

namespace BeDoHave.Data.AccessLayer.Migrations
{
    public partial class Icon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IconColor",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconName",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconColor",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "IconName",
                table: "Pages");
        }
    }
}
