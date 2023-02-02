using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spoonful.Migrations
{
    /// <inheritdoc />
    public partial class updateDriver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehicleNo",
                table: "UserDetails");

            migrationBuilder.AddColumn<double>(
                name: "Commision",
                table: "UserDetails",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "HourlyRate",
                table: "UserDetails",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "UserDetails",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Commision",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "HourlyRate",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "UserDetails");

            migrationBuilder.AddColumn<int>(
                name: "VehicleNo",
                table: "UserDetails",
                type: "int",
                nullable: true);
        }
    }
}
