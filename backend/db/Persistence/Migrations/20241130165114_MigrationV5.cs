using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Users_TeacherId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentUsers_Users_UserId",
                table: "AssignmentUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Users_TeacherId",
                table: "Exercises");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AssignmentUsers",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_AssignmentUsers_UserId",
                table: "AssignmentUsers",
                newName: "IX_AssignmentUsers_StudentId");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Exercises",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "AssignmentUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teacher",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teacher", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_StudentId",
                table: "Exercises",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentUsers_TeacherId",
                table: "AssignmentUsers",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Teacher_TeacherId",
                table: "Assignments",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentUsers_Student_StudentId",
                table: "AssignmentUsers",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentUsers_Teacher_TeacherId",
                table: "AssignmentUsers",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Student_StudentId",
                table: "Exercises",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Teacher_TeacherId",
                table: "Exercises",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Teacher_TeacherId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentUsers_Student_StudentId",
                table: "AssignmentUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentUsers_Teacher_TeacherId",
                table: "AssignmentUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Student_StudentId",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Teacher_TeacherId",
                table: "Exercises");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Teacher");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_StudentId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentUsers_TeacherId",
                table: "AssignmentUsers");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "AssignmentUsers");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "AssignmentUsers",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AssignmentUsers_StudentId",
                table: "AssignmentUsers",
                newName: "IX_AssignmentUsers_UserId");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsTeacher = table.Column<bool>(type: "bit", nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Users_TeacherId",
                table: "Assignments",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentUsers_Users_UserId",
                table: "AssignmentUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Users_TeacherId",
                table: "Exercises",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
