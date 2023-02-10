using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spoonful.Migrations
{
    /// <inheritdoc />
    public partial class Updatingdeliverydatabasesv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Delivery_OrderDetails_OrderDetailsId",
                table: "Delivery");

            migrationBuilder.DropForeignKey(
                name: "FK_Delivery_Rewards_VouchersId",
                table: "Delivery");

            migrationBuilder.DropForeignKey(
                name: "FK_Delivery_Stops_StopsId",
                table: "Delivery");

            migrationBuilder.DropIndex(
                name: "IX_Delivery_VouchersId",
                table: "Delivery");

            migrationBuilder.DropColumn(
                name: "routeId",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "VouchersId",
                table: "Delivery");

            migrationBuilder.DropColumn(
                name: "orderId",
                table: "Delivery");

            migrationBuilder.DropColumn(
                name: "stopId",
                table: "Delivery");

            migrationBuilder.RenameColumn(
                name: "StopsId",
                table: "Delivery",
                newName: "stopsId");

            migrationBuilder.RenameColumn(
                name: "OrderDetailsId",
                table: "Delivery",
                newName: "orderdetailsId");

            migrationBuilder.RenameIndex(
                name: "IX_Delivery_StopsId",
                table: "Delivery",
                newName: "IX_Delivery_stopsId");

            migrationBuilder.RenameIndex(
                name: "IX_Delivery_OrderDetailsId",
                table: "Delivery",
                newName: "IX_Delivery_orderdetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Delivery_OrderDetails_orderdetailsId",
                table: "Delivery",
                column: "orderdetailsId",
                principalTable: "OrderDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Delivery_Stops_stopsId",
                table: "Delivery",
                column: "stopsId",
                principalTable: "Stops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Delivery_OrderDetails_orderdetailsId",
                table: "Delivery");

            migrationBuilder.DropForeignKey(
                name: "FK_Delivery_Stops_stopsId",
                table: "Delivery");

            migrationBuilder.RenameColumn(
                name: "stopsId",
                table: "Delivery",
                newName: "StopsId");

            migrationBuilder.RenameColumn(
                name: "orderdetailsId",
                table: "Delivery",
                newName: "OrderDetailsId");

            migrationBuilder.RenameIndex(
                name: "IX_Delivery_stopsId",
                table: "Delivery",
                newName: "IX_Delivery_StopsId");

            migrationBuilder.RenameIndex(
                name: "IX_Delivery_orderdetailsId",
                table: "Delivery",
                newName: "IX_Delivery_OrderDetailsId");

            migrationBuilder.AddColumn<int>(
                name: "routeId",
                table: "Stops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VouchersId",
                table: "Delivery",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "orderId",
                table: "Delivery",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "stopId",
                table: "Delivery",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_VouchersId",
                table: "Delivery",
                column: "VouchersId");

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
        }
    }
}
