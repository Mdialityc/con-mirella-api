using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConMirellaApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class GroupNavigationRel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "Groups",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlatformId1",
                table: "Groups",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_CategoryId1",
                table: "Groups",
                column: "CategoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_PlatformId1",
                table: "Groups",
                column: "PlatformId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Categories_CategoryId1",
                table: "Groups",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Platforms_PlatformId1",
                table: "Groups",
                column: "PlatformId1",
                principalTable: "Platforms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Categories_CategoryId1",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Platforms_PlatformId1",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_CategoryId1",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_PlatformId1",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "PlatformId1",
                table: "Groups");
        }
    }
}
