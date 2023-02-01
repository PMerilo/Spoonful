using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spoonful.Migrations
{
    /// <inheritdoc />
    public partial class subscriptioncheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SubscriptionCheck",
                table: "OrderDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SubscriptionCheck",
                table: "MealKit",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscriptionCheck",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "SubscriptionCheck",
                table: "MealKit");
        }
    }
}
