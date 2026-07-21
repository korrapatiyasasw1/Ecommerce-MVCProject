using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCDotnetCore.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerAddressId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Order_CustomerAddressId",
                table: "Order",
                column: "CustomerAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_CustomerAddresses_CustomerAddressId",
                table: "Order",
                column: "CustomerAddressId",
                principalTable: "CustomerAddresses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_CustomerAddresses_CustomerAddressId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_CustomerAddressId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CustomerAddressId",
                table: "Order");
        }
    }
}
