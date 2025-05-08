using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace proj_tt.Migrations
{
    /// <inheritdoc />
    public partial class removeaddressIdOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOrders_AppAddresses_AddressId",
                table: "AppOrders");

            migrationBuilder.DropIndex(
                name: "IX_AppOrders_AddressId",
                table: "AppOrders");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "AppOrders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "AppOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppOrders_AddressId",
                table: "AppOrders",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppOrders_AppAddresses_AddressId",
                table: "AppOrders",
                column: "AddressId",
                principalTable: "AppAddresses",
                principalColumn: "Id");
        }
    }
}
