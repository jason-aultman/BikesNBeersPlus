using System;
namespace BikesNBeersMVC.Models
{
    public class Welcome
    {
        public int MaxMiles { get; set; }
        public int ZipCodeStart { get; set; }
        public string ZipCodeEnd { get; set; }
        public DateTime TripDate { get; set; }
        public string AddressStart { get; set; }
        
    }
}
