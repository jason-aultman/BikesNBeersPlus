using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BikesNBeersMVC.Context;
using BikesNBeersMVC.Models;
using BikesNBeersMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BikesNBeersMVC.Controllers
{
    [Route("BrewViewTwo")]
    public class BrewViewTwoController : Controller
    {
        private readonly IBrewHandler _brewHandler;
        private readonly ICoordinateHandler _coordinateHandler;
        private readonly ApplicationDbContext _applicationDbContext;

        public BrewViewTwoController(IBrewHandler brew, ICoordinateHandler coord, ApplicationDbContext context)
        {
            _brewHandler = brew;
            _coordinateHandler = coord;
            _applicationDbContext = context;

        }

        public async Task<IActionResult> WelcomeFrom(Welcome welcome)
        {
            var stops = new List<Stop>();
            var breweryResults = await _brewHandler.GetBreweryByAddress(welcome.AddressStart, welcome.MaxMiles);
            foreach (var brewery in breweryResults.Results)
            {
                stops.Add(new Stop()
                {
                    Name = brewery.Name,
                    Rating = brewery.Rating,
                    Address = brewery.Vicinity,
                    //Phone = brewery.Phone
                    Photo = brewery.photoURL,
                    IsHotel = false,
                    //Lat = brewery.Geometry.Location.Lat
                    //Long = brewery.Geometry.Location.lng
                });
            }

            return Index(stops);
        }

        [Route("index")]
        public IActionResult Index(List<Stop> stops)
        {
            return View(stops);
        }

        [Route("BrewDestGiven")]
        public async Task<IActionResult> BrewDestGiven(Welcome welcome)
        {
            var breweryResults = await _brewHandler.GetBrewery(welcome.ZipCodeEnd);

            var viewModel = new ViewModel();
            viewModel.Breweries = breweryResults;
            return View("Index", viewModel);
        }


       // [HttpPost]
        [Route("selectedBrewView/")]
        public async Task<IActionResult> SelectedBrewView(Stop stop)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var bikerInfo = await _applicationDbContext.BikerInfos
                .FirstOrDefaultAsync(biker => biker.UserId == userId);
            //if(selectedBrewView.tripid == nnull || 0)
            //check if we need to create a new trip or not..is the user adding to an existing
            var trip = new Trip()
            {
                BikerInfoId = bikerInfo.Id
            };

            //keep this trip id around so that y ou can add more stops 
            //add the stop to the trip
            //save the changes to the db
            var coord = await _coordinateHandler.GetCoordinatesByAddress(stop.Address);
            stop.lat = coord.results[0].geometry.location.lat;
            stop.lng = coord.results[0].geometry.location.lng;

            //some check indexing logic for stop number
            //trip.stops.legnth or .count ++ or +1
            //stop.number = the above line

            trip.Stops.Add(stop);
            _applicationDbContext.Add(trip);
            _applicationDbContext.Add<Stop>(stop);
            await _applicationDbContext.SaveChangesAsync();

            //cory remember we need to check if trip will have id at this pt or
            //if we need to get it out of the database
            //var trip = blahcontet getmetrip
            return View(trip);
        }
        //[HttpPost]
        //[Route("selectedBrewView")]
        //public IActionResult selectedBrewView(ViewModel selectedBrewViewModel)
        //{
        //    var lstSelected = selectedBrewViewModel.Breweries.Results.Where(x => x.selected == 1).ToList();
        //    return View(lstSelected);
        //}

        //public IActionResult Index(int zip, double miles)
        //{
        //    var breweryResults = _brewHandler.GetBrewery(zip, miles);
        //    var viewModel = new ViewModel();
        //    viewModel.Breweries = breweryResults;
        //    return View();
        //}
    }
}

