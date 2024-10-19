using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConMirellaApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixValuateDbSetName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Valuations_Groups_GroupId",
                table: "Valuations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Valuations",
                table: "Valuations");

            migrationBuilder.RenameTable(
                name: "Valuations",
                newName: "Valuates");

            migrationBuilder.RenameIndex(
                name: "IX_Valuations_GroupId",
                table: "Valuates",
                newName: "IX_Valuates_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Valuates",
                table: "Valuates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Valuates_Groups_GroupId",
                table: "Valuates",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Valuates_Groups_GroupId",
                table: "Valuates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Valuates",
                table: "Valuates");

            migrationBuilder.RenameTable(
                name: "Valuates",
                newName: "Valuations");

            migrationBuilder.RenameIndex(
                name: "IX_Valuates_GroupId",
                table: "Valuations",
                newName: "IX_Valuations_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Valuations",
                table: "Valuations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Valuations_Groups_GroupId",
                table: "Valuations",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
