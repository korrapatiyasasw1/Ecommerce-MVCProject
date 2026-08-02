using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCDotnetCore.Migrations
{
    /// <inheritdoc />
    public partial class AddedMailsindex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailOtps_Users_UserId",
                table: "EmailOtps");

            migrationBuilder.DropIndex(
                name: "IX_EmailOtps_UserId",
                table: "EmailOtps");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EmailOtps");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "EmailOtps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EmailOtps_UserId",
                table: "EmailOtps",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailOtps_Users_UserId",
                table: "EmailOtps",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
