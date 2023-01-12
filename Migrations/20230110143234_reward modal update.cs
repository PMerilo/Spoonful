using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spoonful.Migrations
{
    /// <inheritdoc />
    public partial class rewardmodalupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "MenuPreference",
                table: "Rewards");

            migrationBuilder.RenameColumn(
                name: "Tags",
                table: "Rewards",
                newName: "voucherCode");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Rewards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "discountAmount",
                table: "Rewards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "expiryDate",
                table: "Rewards",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "discountAmount",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "expiryDate",
                table: "Rewards");

            migrationBuilder.RenameColumn(
                name: "voucherCode",
                table: "Rewards",
                newName: "Tags");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Rewards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "Rewards",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MenuPreference",
                table: "Rewards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
