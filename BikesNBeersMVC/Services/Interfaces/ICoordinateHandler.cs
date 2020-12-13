using System;
using System.Threading.Tasks;
using BikesNBeersMVC.Models;

namespace BikesNBeersMVC.Services
{
    public interface ICoordinateHandler
    {
        Task<Coordinate> GetCoordinates(string zipCode);
        Task<Coordinate> GetCoordinatesByAddress(string address);
    }
    
}
