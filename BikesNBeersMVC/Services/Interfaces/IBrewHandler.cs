using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesNBeersMVC.Services
{
    public interface IBrewHandler
    {
        Task<BreweryResponse> GetBrewery(string zipcode);
        Task<BreweryResponse> GetBrewery(string zipcode, double distance_in_miles);
        Task<BreweryResponse> GetBreweryByAddress(string address, double distanceInMiles);
    }
}
