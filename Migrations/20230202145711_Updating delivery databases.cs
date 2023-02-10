using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spoonful.Migrations
{
    /// <inheritdoc />
    public partial class Updatingdeliverydatabases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TextContent",
                table: "VoucherEmail");

            migrationBuilder.DropColumn(
                name: "driverId",
                table: "Route");

            migrationBuilder.RenameColumn(
                name: "voucherId",
                table: "VoucherEmail",
                newName: "vouchersId");

            migrationBuilder.AddColumn<int>(
                name: "RoutesId",
                table: "Stops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderDetailsId",
                table: "Delivery",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StopsId",
                table: "Delivery",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VouchersId",
                table: "Delivery",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoutesId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VoucherEmail_vouchersId",
                table: "VoucherEmail",
                column: "vouchersId");

            migrationBuilder.CreateIndex(
                name: "IX_Stops_RoutesId",
                table: "Stops",
                column: "RoutesId");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_OrderDetailsId",
                table: "Delivery",
                column: "OrderDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_StopsId",
                table: "Delivery",
                column: "StopsId");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_VouchersId",
                table: "Delivery",
                column: "VouchersId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Delivery_OrderDetails_OrderDetailsId",
                table: "Delivery",
                column: "OrderDetailsId",
                principalTable: "OrderDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Delivery_Rewards_VouchersId",
                table: "Delivery",
                column: "VouchersId",
                principalTable: "Rewards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Delivery_Stops_StopsId",
                table: "Delivery",
                column: "StopsId",
                principalTable: "Stops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stops_Route_RoutesId",
                table: "Stops",
                column: "RoutesId",
                principalTable: "Route",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherEmail_Rewards_vouchersId",
                table: "VoucherEmail",
                column: "vouchersId",
                principalTable: "Rewards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Route_RoutesId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Delivery_OrderDetails_OrderDetailsId",
                table: "Delivery");

            migrationBuilder.DropForeignKey(
                name: "FK_Delivery_Rewards_VouchersId",
                table: "Delivery");

            migrationBuilder.DropForeignKey(
                name: "FK_Delivery_Stops_StopsId",
                table: "Delivery");

            migrationBuilder.DropForeignKey(
                name: "FK_Stops_Route_RoutesId",
                table: "Stops");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherEmail_Rewards_vouchersId",
                table: "VoucherEmail");

            migrationBuilder.DropIndex(
                name: "IX_VoucherEmail_vouchersId",
                table: "VoucherEmail");

            migrationBuilder.DropIndex(
                name: "IX_Stops_RoutesId",
                table: "Stops");

            migrationBuilder.DropIndex(
                name: "IX_Delivery_OrderDetailsId",
                table: "Delivery");

            migrationBuilder.DropIndex(
                name: "IX_Delivery_StopsId",
                table: "Delivery");

            migrationBuilder.DropIndex(
                name: "IX_Delivery_VouchersId",
                table: "Delivery");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RoutesId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RoutesId",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "OrderDetailsId",
                table: "Delivery");

            migrationBuilder.DropColumn(
                name: "StopsId",
                table: "Delivery");

            migrationBuilder.DropColumn(
                name: "VouchersId",
                table: "Delivery");

            migrationBuilder.DropColumn(
                name: "RoutesId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "vouchersId",
                table: "VoucherEmail",
                newName: "voucherId");

            migrationBuilder.AddColumn<string>(
                name: "TextContent",
                table: "VoucherEmail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "driverId",
                table: "Route",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
