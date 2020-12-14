using Microsoft.EntityFrameworkCore.Migrations;

namespace BikesNBeersMVC.Migrations
{
    public partial class changetripmilestodouble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "TripMiles",
                table: "Trips",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TripMiles",
                table: "Trips",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
