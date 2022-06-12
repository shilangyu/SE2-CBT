using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CbtBackend.Migrations
{
    public partial class MoodtestResultsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResultsTableId",
                table: "Evaluations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MudTestResultsTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntryCategory = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MudTestResultsTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MudTestResultsTableEntry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScoreFrom = table.Column<int>(type: "integer", nullable: false),
                    ScoreTo = table.Column<int>(type: "integer", nullable: false),
                    EntryName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    MudTestResultsTableId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MudTestResultsTableEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MudTestResultsTableEntry_MudTestResultsTable_MudTestResults~",
                        column: x => x.MudTestResultsTableId,
                        principalTable: "MudTestResultsTable",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_ResultsTableId",
                table: "Evaluations",
                column: "ResultsTableId");

            migrationBuilder.CreateIndex(
                name: "IX_MudTestResultsTableEntry_MudTestResultsTableId",
                table: "MudTestResultsTableEntry",
                column: "MudTestResultsTableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluations_MudTestResultsTable_ResultsTableId",
                table: "Evaluations",
                column: "ResultsTableId",
                principalTable: "MudTestResultsTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluations_MudTestResultsTable_ResultsTableId",
                table: "Evaluations");

            migrationBuilder.DropTable(
                name: "MudTestResultsTableEntry");

            migrationBuilder.DropTable(
                name: "MudTestResultsTable");

            migrationBuilder.DropIndex(
                name: "IX_Evaluations_ResultsTableId",
                table: "Evaluations");

            migrationBuilder.DropColumn(
                name: "ResultsTableId",
                table: "Evaluations");
        }
    }
}
