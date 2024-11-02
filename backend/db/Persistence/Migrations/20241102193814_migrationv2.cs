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
                name: "FK_Users_Assignments_AssignmentsId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AssignmentsId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AssignmentsId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "AssignmentUsers",
                columns: table => new
                {
                    AssignmentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentUsers", x => new { x.AssignmentId, x.UserId });
                    table.ForeignKey(
                        name: "FK_AssignmentUsers_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentUsers_UserId",
                table: "AssignmentUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignmentUsers");

            migrationBuilder.AddColumn<int>(
                name: "AssignmentsId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AssignmentsId",
                table: "Users",
                column: "AssignmentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Assignments_AssignmentsId",
                table: "Users",
                column: "AssignmentsId",
                principalTable: "Assignments",
                principalColumn: "Id");
        }
    }
}
