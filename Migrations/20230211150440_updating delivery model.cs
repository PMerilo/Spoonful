using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spoonful.Migrations
{
    /// <inheritdoc />
    public partial class updatingdeliverymodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Delivery",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "Delivery");
        }
    }
}
