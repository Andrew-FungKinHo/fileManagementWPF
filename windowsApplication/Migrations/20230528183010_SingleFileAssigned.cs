using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace windowsApplication.Migrations
{
    /// <inheritdoc />
    public partial class SingleFileAssigned : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppFileUser");

            migrationBuilder.AddColumn<int>(
                name: "FileAssignedId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_FileAssignedId",
                table: "Users",
                column: "FileAssignedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AppFiles_FileAssignedId",
                table: "Users",
                column: "FileAssignedId",
                principalTable: "AppFiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_AppFiles_FileAssignedId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_FileAssignedId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FileAssignedId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "AppFileUser",
                columns: table => new
                {
                    FilesAssignedId = table.Column<int>(type: "int", nullable: false),
                    UsersAssignedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppFileUser", x => new { x.FilesAssignedId, x.UsersAssignedId });
                    table.ForeignKey(
                        name: "FK_AppFileUser_AppFiles_FilesAssignedId",
                        column: x => x.FilesAssignedId,
                        principalTable: "AppFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppFileUser_Users_UsersAssignedId",
                        column: x => x.UsersAssignedId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppFileUser_UsersAssignedId",
                table: "AppFileUser",
                column: "UsersAssignedId");
        }
    }
}
