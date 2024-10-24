using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignmentsId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: true),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateDue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignments_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_AssignmentsId",
                table: "Users",
                column: "AssignmentsId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_ExerciseId",
                table: "Assignments",
                column: "ExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Assignments_AssignmentsId",
                table: "Users",
                column: "AssignmentsId",
                principalTable: "Assignments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Assignments_AssignmentsId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Users_AssignmentsId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AssignmentsId",
                table: "Users");
        }
    }
}
