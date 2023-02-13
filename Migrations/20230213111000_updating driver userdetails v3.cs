using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spoonful.Migrations
{
    /// <inheritdoc />
    public partial class updatingdriveruserdetailsv3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoutesId",
                table: "UserDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_RoutesId",
                table: "UserDetails",
                column: "RoutesId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDetails_Route_RoutesId",
                table: "UserDetails",
                column: "RoutesId",
                principalTable: "Route",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDetails_Route_RoutesId",
                table: "UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserDetails_RoutesId",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "RoutesId",
                table: "UserDetails");
        }
    }
}
