using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProfileDocksService.Migrations
{
    /// <inheritdoc />
    public partial class Scans1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EducationScan_EducationDocuments_EducationDocumentId",
                table: "EducationScan");

            migrationBuilder.DropForeignKey(
                name: "FK_PassportScan_PassportDocuments_PassportDocumentId",
                table: "PassportScan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PassportScan",
                table: "PassportScan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EducationScan",
                table: "EducationScan");

            migrationBuilder.RenameTable(
                name: "PassportScan",
                newName: "PassportScans");

            migrationBuilder.RenameTable(
                name: "EducationScan",
                newName: "EducationScans");

            migrationBuilder.RenameIndex(
                name: "IX_PassportScan_PassportDocumentId",
                table: "PassportScans",
                newName: "IX_PassportScans_PassportDocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_EducationScan_EducationDocumentId",
                table: "EducationScans",
                newName: "IX_EducationScans_EducationDocumentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PassportScans",
                table: "PassportScans",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EducationScans",
                table: "EducationScans",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationScans_EducationDocuments_EducationDocumentId",
                table: "EducationScans",
                column: "EducationDocumentId",
                principalTable: "EducationDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PassportScans_PassportDocuments_PassportDocumentId",
                table: "PassportScans",
                column: "PassportDocumentId",
                principalTable: "PassportDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EducationScans_EducationDocuments_EducationDocumentId",
                table: "EducationScans");

            migrationBuilder.DropForeignKey(
                name: "FK_PassportScans_PassportDocuments_PassportDocumentId",
                table: "PassportScans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PassportScans",
                table: "PassportScans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EducationScans",
                table: "EducationScans");

            migrationBuilder.RenameTable(
                name: "PassportScans",
                newName: "PassportScan");

            migrationBuilder.RenameTable(
                name: "EducationScans",
                newName: "EducationScan");

            migrationBuilder.RenameIndex(
                name: "IX_PassportScans_PassportDocumentId",
                table: "PassportScan",
                newName: "IX_PassportScan_PassportDocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_EducationScans_EducationDocumentId",
                table: "EducationScan",
                newName: "IX_EducationScan_EducationDocumentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PassportScan",
                table: "PassportScan",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EducationScan",
                table: "EducationScan",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationScan_EducationDocuments_EducationDocumentId",
                table: "EducationScan",
                column: "EducationDocumentId",
                principalTable: "EducationDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PassportScan_PassportDocuments_PassportDocumentId",
                table: "PassportScan",
                column: "PassportDocumentId",
                principalTable: "PassportDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
