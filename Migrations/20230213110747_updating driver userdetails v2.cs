using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spoonful.Migrations
{
    /// <inheritdoc />
    public partial class updatingdriveruserdetailsv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDetails_Route_RoutesId",
                table: "UserDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDetails_Route_RoutesId1",
                table: "UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserDetails_RoutesId",
                table: "UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserDetails_RoutesId1",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "RoutesId",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "RoutesId1",
                table: "UserDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoutesId",
                table: "UserDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoutesId1",
                table: "UserDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_RoutesId",
                table: "UserDetails",
                column: "RoutesId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_RoutesId1",
                table: "UserDetails",
                column: "RoutesId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDetails_Route_RoutesId",
                table: "UserDetails",
                column: "RoutesId",
                principalTable: "Route",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDetails_Route_RoutesId1",
                table: "UserDetails",
                column: "RoutesId1",
                principalTable: "Route",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
