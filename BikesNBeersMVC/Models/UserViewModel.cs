using System;
using System.Collections.Generic;

namespace BikesNBeersMVC.Models
{
    public class UserViewModel
    {
        public int TotalMiles { get; set; }
        public List<Badges> Badges { get; set; }
        public List<Trip> Trips { get; set; }

    }
}
