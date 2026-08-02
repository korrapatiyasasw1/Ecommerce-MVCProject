using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCDotnetCore.Migrations
{
    /// <inheritdoc />
    public partial class AddedMailsindexse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEmailVerified",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailOtps_Users_UserId",
                table: "EmailOtps");

            migrationBuilder.DropIndex(
                name: "IX_EmailOtps_UserId",
                table: "EmailOtps");

            migrationBuilder.DropColumn(
                name: "IsEmailVerified",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EmailOtps");
        }
    }
}
