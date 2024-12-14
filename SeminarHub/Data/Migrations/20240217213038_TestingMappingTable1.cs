using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeminarHub.Data.Migrations
{
    public partial class TestingMappingTable1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seminars_Categories_CategoryId",
                table: "Seminars");

            migrationBuilder.AddForeignKey(
                name: "FK_Seminars_Categories_CategoryId",
                table: "Seminars",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seminars_Categories_CategoryId",
                table: "Seminars");

            migrationBuilder.AddForeignKey(
                name: "FK_Seminars_Categories_CategoryId",
                table: "Seminars",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
