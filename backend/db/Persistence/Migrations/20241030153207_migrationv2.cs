using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class migrationv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Teachers_TeacherId",
                table: "Exercises");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Teachers_TeacherId",
                table: "Exercises",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Teachers_TeacherId",
                table: "Exercises");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Teachers_TeacherId",
                table: "Exercises",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id");
        }
    }
}
