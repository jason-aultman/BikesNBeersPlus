using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesNBeersMVC.Models
{
    public class ViewModel
    {
        public BreweryResponse Breweries { get; set; }
        public HotelResponse HotelResponses { get; set; }
        public Route[] Routes { get; set; }
    }
}
