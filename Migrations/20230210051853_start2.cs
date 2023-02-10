using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spoonful.Migrations
{
    /// <inheritdoc />
    public partial class start2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Delivery_OrderDetails_OrderDetailsId1",
                table: "Delivery");

            migrationBuilder.DropIndex(
                name: "IX_Delivery_OrderDetailsId1",
                table: "Delivery");

            migrationBuilder.DropColumn(
                name: "OrderDetailsId1",
                table: "Delivery");

            migrationBuilder.AlterColumn<string>(
                name: "OrderDetailsId",
                table: "Delivery",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_OrderDetailsId",
                table: "Delivery",
                column: "OrderDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Delivery_OrderDetails_OrderDetailsId",
                table: "Delivery",
                column: "OrderDetailsId",
                principalTable: "OrderDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Delivery_OrderDetails_OrderDetailsId",
                table: "Delivery");

            migrationBuilder.DropIndex(
                name: "IX_Delivery_OrderDetailsId",
                table: "Delivery");

            migrationBuilder.AlterColumn<int>(
                name: "OrderDetailsId",
                table: "Delivery",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "OrderDetailsId1",
                table: "Delivery",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_OrderDetailsId1",
                table: "Delivery",
                column: "OrderDetailsId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Delivery_OrderDetails_OrderDetailsId1",
                table: "Delivery",
                column: "OrderDetailsId1",
                principalTable: "OrderDetails",
                principalColumn: "Id");
        }
    }
}
