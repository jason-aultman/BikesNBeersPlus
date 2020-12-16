using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikesNBeersMVC.Context;
using BikesNBeersMVC.Models;
using BikesNBeersMVC.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BikesNBeersMVC.Controllers
{
   // [Authorize]
    public class WelcomeController : Controller
    {
        private ApplicationDbContext _applicationDbContext;
        private readonly IStopHandler _stopHandler;
        public WelcomeController(ApplicationDbContext applicationDbContext, IStopHandler stopHandler)
        {
            _stopHandler = stopHandler;
            _applicationDbContext = applicationDbContext;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

       // [Authorize]
        public IActionResult Welcome(StopSearchViewModel stopSearchViewModel)
        {
            var getTrip = _applicationDbContext.Trips.LastOrDefault();
            var coords = _stopHandler.GetCoordinatesByAddress(stopSearchViewModel.AddressStart).GetAwaiter().GetResult();
            var homeStop = new Stop()
            {
                Address = stopSearchViewModel.AddressStart,
                IsHotel = false,
                StopOrderNumber = 1,
                StartingLatitiude = coords.results[0].geometry.location.lat,
                StartingLongitude = coords.results[0].geometry.location.lng
                
            };
            getTrip.Stops.Add(homeStop);
            _applicationDbContext.Add(getTrip);
            _applicationDbContext.SaveChangesAsync();
            return View();
        }
    }
}
