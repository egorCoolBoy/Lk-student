using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProfileDocksService.Migrations
{
    /// <inheritdoc />
    public partial class EdLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Level",
                table: "EducationDocuments",
                newName: "LevelName");

            migrationBuilder.AddColumn<int>(
                name: "LevelId",
                table: "EducationDocuments",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "EducationDocuments");

            migrationBuilder.RenameColumn(
                name: "LevelName",
                table: "EducationDocuments",
                newName: "Level");
        }
    }
}
