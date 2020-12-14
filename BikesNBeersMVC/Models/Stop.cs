using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesNBeersMVC.Models
{
    public class Stop 
    {
        public int Id { get; set; }
        public bool IsHotel { get; set; }
        public string Name { get; set; }
        public double Rating { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Photo { get; set; }

        //starting lat and starting long are based on the user search not based on the last stop lat or long
        public double StartingLatitiude { get; set; }
        public double StartingLongitude { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public int StopOrderNumber { get; set; }

        public int TripId { get; set; }
    }


}
