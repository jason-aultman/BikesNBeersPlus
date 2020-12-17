using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikesNBeersMVC.Context;
using BikesNBeersMVC.Models;
using BikesNBeersMVC.Services;
using BikesNBeersMVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BikesNBeersMVC.Controllers
{
    public class MapViewController : Controller
    {
        public readonly IStopHandler _stopHandler;
        public readonly ApplicationDbContext _applicationDbContext;

        public MapViewController(IStopHandler stopHandler, ApplicationDbContext dbContext)
        {
            _applicationDbContext = dbContext;
            _stopHandler = stopHandler;
        }
      //  [HttpPost]
        public IActionResult Index(int tripId)
        {
            var stopList = new List<Stop>();
            var trips = _applicationDbContext.Trips.Where(_ => _.Id == tripId).ToList();
            var thisTrip = trips[0];
            stopList = _applicationDbContext.Stops.Where(_ => _.TripId == tripId).ToList();
            
           
            return View(thisTrip);
        }
    }
}