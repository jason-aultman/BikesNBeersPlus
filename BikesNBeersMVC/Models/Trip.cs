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
        public int TripMiles { get; set; }
        public DateTime TripDate { get; set; }
        public int TripRating { get; set; }
        public int BikerInfoId { get; set; }
        public BikerInfo BikerInfo { get; set; }

        public List<Stop> Stops { get; set; } = new List<Stop>();
    }
}
