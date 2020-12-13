using Microsoft.EntityFrameworkCore.Migrations;

namespace BikesNBeersMVC.Migrations
{
    public partial class updatedStopModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "Stops",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Stops");
        }
    }
}
