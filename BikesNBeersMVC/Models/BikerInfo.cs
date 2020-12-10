using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesNBeersMVC.Models
{
    public class BikerInfo
    {
        public int Id { get; set; }
        public int TotalMiles { get; set; }

        public string UserId { get; set; }

        public List<Trip> Trips { get; set; } = new List<Trip>();
        public List<Badges> Badges { get; set; } = new List<Badges>();
//foreign key reference to Identity
     //   public IdentityUser User { get; set; }
    }
}
