using System;
using BikesNBeersMVC.Models;

namespace BikesNBeersMVC.Services
{
    public interface ICoordinateHandler
    {
        public Coordinate GetCoordinates(int zipCode);
    }
    
}
