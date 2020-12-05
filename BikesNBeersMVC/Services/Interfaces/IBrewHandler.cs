using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesNBeersMVC.Services
{
    public interface IBrewHandler
    {
        public BreweryResponse GetBrewery(int zipcode);

    }
}
