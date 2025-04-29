using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exam.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_UserExams_UserExamId1",
                table: "UserAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UserAnswers_UserExamId1",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "UserExamId1",
                table: "UserAnswers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserExamId1",
                table: "UserAnswers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_UserExamId1",
                table: "UserAnswers",
                column: "UserExamId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_UserExams_UserExamId1",
                table: "UserAnswers",
                column: "UserExamId1",
                principalTable: "UserExams",
                principalColumn: "Id");
        }
    }
}
