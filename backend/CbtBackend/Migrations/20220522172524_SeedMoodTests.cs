using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbtBackend.Migrations
{
    public partial class SeedMoodTests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Evaluations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name", "Question1", "Question2", "Question3", "Question4", "Question5" },
                values: new object[] { "A test to check your current depression levels", "Depression", "Sad or down in the dumps", "Discouraged or hopeless", "Low in self-esteem, inferior, or worthless", "Unmotivated to do things", "Decreased pleasure or satisfaction in life" });

            migrationBuilder.InsertData(
                table: "Evaluations",
                columns: new[] { "Id", "Description", "Name", "Question1", "Question2", "Question3", "Question4", "Question5" },
                values: new object[,]
                {
                    { 2, "A test to check your current anxiety levels", "Anxiety", "Anxious", "Frightened", "Worrying about things", "Tense or on edge", "Nervous" },
                    { 3, "A test to check your current addictions levels", "Addictions", "Sometimes I crave drugs or alcohol", "Sometimes I have the urge to use drugs or alcohol", "Sometimes it's hard to resist the urge to use drugs or alcohol", "Sometimes I struggle with the temptaion to use drugs or alcohol", "Nervous" },
                    { 4, "A test to check your current anger levels", "Anger", "Frustrated", "Annoyed", "Resentful", "Angry", "Irritated" },
                    { 5, "A test to check your current relationship levels", "Relationships", "Communication and openness", "Resolving conflicts", "Degree of affection and caring", "Intimacy and closeness", "Overall satisfaction" },
                    { 6, "A test to check your current happiness levels", "Happiness", "Happy and joyful", "Hopeful and optimistic", "Worthwhile, high self-esteem", "Motivated, productive", "Pleased and satisfied with life" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Evaluations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Evaluations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Evaluations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Evaluations",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Evaluations",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "Evaluations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name", "Question1", "Question2", "Question3", "Question4", "Question5" },
                values: new object[] { "example test", "example test", "example q1", "example q2", "example q3", "example q4", "example q5" });
        }
    }
}
