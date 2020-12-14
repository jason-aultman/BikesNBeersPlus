using Microsoft.EntityFrameworkCore.Migrations;

namespace BikesNBeersMVC.Migrations
{
    public partial class upadtedStopandTripModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "StartingLat",
                table: "Trips",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "StartingLong",
                table: "Trips",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "StartingLatitiude",
                table: "Stops",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "StartingLongitude",
                table: "Stops",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartingLat",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "StartingLong",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "StartingLatitiude",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "StartingLongitude",
                table: "Stops");
        }
    }
}
