using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace proj_tt.Migrations
{
    /// <inheritdoc />
    public partial class AddLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
