using System.Collections.Generic;
using System.Threading.Tasks;
using BikesNBeersMVC.Models;

namespace BikesNBeersMVC.Services.Interfaces
{
    public interface IStopHandler
    {
        Task<List<Stop>> GetStopByAddress(string address, double distance_in_miles = 0, string type = "brewery");
        Task<Coordinate> GetCoordinates(string zipCode);
        Task<Coordinate> GetCoordinatesByAddress(string address);
        Task<double> GetDistance(double startingLong, double startingLat, double endingLong, double endingLat);
    }
}
