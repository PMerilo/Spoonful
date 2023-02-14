using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spoonful.Migrations
{
    /// <inheritdoc />
    public partial class updatingdriveruserdetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Route_RoutesId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RoutesId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RoutesId",
                table: "AspNetUsers");

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

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Route",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Route",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "RoutesId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RoutesId",
                table: "AspNetUsers",
                column: "RoutesId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Route_RoutesId",
                table: "AspNetUsers",
                column: "RoutesId",
                principalTable: "Route",
                principalColumn: "Id");
        }
    }
}
