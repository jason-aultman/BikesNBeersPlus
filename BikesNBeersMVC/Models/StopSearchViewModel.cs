using System;
namespace BikesNBeersMVC.Models
{
    public class StopSearchViewModel
    {
        public int MaxMiles { get; set; }
        public int ZipCodeStart { get; set; }
        public string ZipCodeEnd { get; set; }
        public string TripName { get; set; }
        public DateTime TripDate { get; set; } = DateTime.UtcNow; //default to todays date unless otherwise specified
        public string AddressStart { get; set; }
        public string StopType { get; set; } = "Brewery"; //default to brewery unless otherwise specified
        public int TripId { get; set; }
    }
}
