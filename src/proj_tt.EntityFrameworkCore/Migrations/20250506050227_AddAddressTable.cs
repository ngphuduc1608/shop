using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace proj_tt.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "AppOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    AddressLine = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppAddresses_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppOrders_AddressId",
                table: "AppOrders",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAddresses_UserId",
                table: "AppAddresses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppOrders_AppAddresses_AddressId",
                table: "AppOrders",
                column: "AddressId",
                principalTable: "AppAddresses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOrders_AppAddresses_AddressId",
                table: "AppOrders");

            migrationBuilder.DropTable(
                name: "AppAddresses");

            migrationBuilder.DropIndex(
                name: "IX_AppOrders_AddressId",
                table: "AppOrders");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "AppOrders");
        }
    }
}
