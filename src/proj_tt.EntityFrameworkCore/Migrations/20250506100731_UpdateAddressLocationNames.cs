using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace proj_tt.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAddressLocationNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistrictCode",
                table: "AppAddresses");

            migrationBuilder.DropColumn(
                name: "ProvinceCode",
                table: "AppAddresses");

            migrationBuilder.DropColumn(
                name: "WardCode",
                table: "AppAddresses");

            migrationBuilder.AddColumn<string>(
                name: "DistrictName",
                table: "AppAddresses",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProvinceName",
                table: "AppAddresses",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WardName",
                table: "AppAddresses",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistrictName",
                table: "AppAddresses");

            migrationBuilder.DropColumn(
                name: "ProvinceName",
                table: "AppAddresses");

            migrationBuilder.DropColumn(
                name: "WardName",
                table: "AppAddresses");

            migrationBuilder.AddColumn<string>(
                name: "DistrictCode",
                table: "AppAddresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProvinceCode",
                table: "AppAddresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WardCode",
                table: "AppAddresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
