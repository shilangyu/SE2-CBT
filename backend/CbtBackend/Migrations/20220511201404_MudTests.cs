using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CbtBackend.Migrations
{
    public partial class MudTests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Evaluations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Question1 = table.Column<string>(type: "text", nullable: false),
                    Question2 = table.Column<string>(type: "text", nullable: false),
                    Question3 = table.Column<string>(type: "text", nullable: false),
                    Question4 = table.Column<string>(type: "text", nullable: false),
                    Question5 = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evaluations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EvaluationResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuthorId = table.Column<string>(type: "text", nullable: false),
                    Submitted = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EvaluationId = table.Column<int>(type: "integer", nullable: false),
                    Response1 = table.Column<int>(type: "integer", nullable: false),
                    Response2 = table.Column<int>(type: "integer", nullable: false),
                    Response3 = table.Column<int>(type: "integer", nullable: false),
                    Response4 = table.Column<int>(type: "integer", nullable: false),
                    Response5 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvaluationResponses_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EvaluationResponses_Evaluations_EvaluationId",
                        column: x => x.EvaluationId,
                        principalTable: "Evaluations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Evaluations",
                columns: new[] { "Id", "Description", "Name", "Question1", "Question2", "Question3", "Question4", "Question5" },
                values: new object[] { 1, "example test", "example test", "example q1", "example q2", "example q3", "example q4", "example q5" });

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationResponses_AuthorId",
                table: "EvaluationResponses",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationResponses_EvaluationId",
                table: "EvaluationResponses",
                column: "EvaluationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvaluationResponses");

            migrationBuilder.DropTable(
                name: "Evaluations");
        }
    }
}
