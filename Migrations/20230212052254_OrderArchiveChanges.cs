using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spoonful.Migrations
{
    /// <inheritdoc />
    public partial class OrderArchiveChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Archived",
                table: "MealKit");

            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "MenuItem",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Archived",
                table: "MenuItem");

            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "MealKit",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
