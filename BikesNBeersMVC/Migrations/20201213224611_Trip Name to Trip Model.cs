using Microsoft.EntityFrameworkCore.Migrations;

namespace BikesNBeersMVC.Migrations
{
    public partial class TripNametoTripModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TripName",
                table: "Trips",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TripName",
                table: "Trips");
        }
    }
}
