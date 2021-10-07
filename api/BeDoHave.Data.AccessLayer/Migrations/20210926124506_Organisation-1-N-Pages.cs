using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeDoHave.Data.AccessLayer.Migrations
{
    public partial class Organisation1NPages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganisationPage");

            migrationBuilder.AddColumn<int>(
                name: "OrganisationId",
                table: "Pages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pages_OrganisationId",
                table: "Pages",
                column: "OrganisationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pages_Organisations_OrganisationId",
                table: "Pages",
                column: "OrganisationId",
                principalTable: "Organisations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pages_Organisations_OrganisationId",
                table: "Pages");

            migrationBuilder.DropIndex(
                name: "IX_Pages_OrganisationId",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "Pages");

            migrationBuilder.CreateTable(
                name: "OrganisationPage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Direct = table.Column<bool>(type: "bit", nullable: false),
                    OrganisationId = table.Column<int>(type: "int", nullable: false),
                    PageId = table.Column<int>(type: "int", nullable: false),
                    PageTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganisationPage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganisationPage_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganisationPage_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationPage_OrganisationId",
                table: "OrganisationPage",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationPage_PageId",
                table: "OrganisationPage",
                column: "PageId");
        }
    }
}
