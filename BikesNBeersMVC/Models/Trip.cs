using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesNBeersMVC.Models
{
    public class Trip
    {
        public int Id { get; set; }
        public string TripName { get; set; }
        public double TripMiles { get; set; }
        public DateTime TripDate { get; set; }
        public int TripRating { get; set; }

        public double StartingLat { get; set; }
        public double StartingLong { get; set; }

        //this is a trap because the distance between starting and ending would not account for each stop and the distance between stops
        //public double EndingLat { get; set; }
        //public double EndingLong { get; set; }
        public int BikerInfoId { get; set; }
        public BikerInfo BikerInfo { get; set; }

        public List<Stop> Stops { get; set; } = new List<Stop>();
    }
}
