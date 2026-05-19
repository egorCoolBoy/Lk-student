using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProfileDocksService.Migrations
{
    /// <inheritdoc />
    public partial class EdLevels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InstitutionName",
                table: "EducationDocuments",
                newName: "EducationName");

            migrationBuilder.AddColumn<Guid>(
                name: "EducationTypeId",
                table: "EducationDocuments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EducationTypeId",
                table: "EducationDocuments");

            migrationBuilder.RenameColumn(
                name: "EducationName",
                table: "EducationDocuments",
                newName: "InstitutionName");
        }
    }
}
