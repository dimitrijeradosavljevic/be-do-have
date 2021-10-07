using Microsoft.EntityFrameworkCore.Migrations;

namespace BeDoHave.Data.AccessLayer.Migrations
{
    public partial class RemoveTitlesFromPageLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AncestorPageTitle",
                table: "PageLink");

            migrationBuilder.DropColumn(
                name: "DescendantPageTitle",
                table: "PageLink");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AncestorPageTitle",
                table: "PageLink",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescendantPageTitle",
                table: "PageLink",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
