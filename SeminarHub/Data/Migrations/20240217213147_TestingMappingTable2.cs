using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeminarHub.Data.Migrations
{
    public partial class TestingMappingTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeminarsParticipants_Seminars_SeminarId1",
                table: "SeminarsParticipants");

            migrationBuilder.DropIndex(
                name: "IX_SeminarsParticipants_SeminarId1",
                table: "SeminarsParticipants");

            migrationBuilder.DropColumn(
                name: "SeminarId1",
                table: "SeminarsParticipants");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SeminarId1",
                table: "SeminarsParticipants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeminarsParticipants_SeminarId1",
                table: "SeminarsParticipants",
                column: "SeminarId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SeminarsParticipants_Seminars_SeminarId1",
                table: "SeminarsParticipants",
                column: "SeminarId1",
                principalTable: "Seminars",
                principalColumn: "Id");
        }
    }
}
