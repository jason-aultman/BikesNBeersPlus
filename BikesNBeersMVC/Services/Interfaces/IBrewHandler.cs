using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesNBeersMVC.Services
{
    public interface IBrewHandler
    {
        public BreweryResponse GetBrewery(string zipcode);
        public BreweryResponse GetBrewery(string zipcode, double distance_in_miles);
        public BreweryResponse GetBreweryByAddress(string address, double distanceInMiles);
    }
}
