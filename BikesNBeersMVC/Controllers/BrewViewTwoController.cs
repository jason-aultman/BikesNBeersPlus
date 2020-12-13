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
    [Route("Index")]
    public IActionResult Index(Welcome welcome)
    {
      var breweryResults = _brewHandler.GetBreweryByAddress(welcome.AddressStart, welcome.MaxMiles);
      var viewModel = new ViewModel();
      viewModel.Breweries = breweryResults;
      return View(viewModel);
            // var ListofStop = ... fetch data from db or ...
            //return View(ListofStop);
            //list does not exist in model
        }
        [Route("BrewDestGiven")]
    public IActionResult BrewDestGiven(Welcome welcome)
    {
      var breweryResults = _brewHandler.GetBrewery(welcome.ZipCodeEnd);


      var viewModel = new ViewModel();
      viewModel.Breweries = breweryResults;
      return View("Index", viewModel);
    }
        [HttpPost]
        [Route("selectedBrewView/")]
        public async Task<IActionResult> selectedBrewView(ViewModel selectedBrewViewModel)
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
            var lstSelected = selectedBrewViewModel.Breweries.Results.Where(x => x.selected == 1).ToList();
            var newStop = new Stop() { IsHotel = false, Name = lstSelected[0].Name, Address = lstSelected[0].Vicinity, Phone = " ", Photo = lstSelected[0].photoURL, StopOrderNumber = 1 };
            var coord = _coordinateHandler.GetCoordinatesByAddress(lstSelected[0].Vicinity);
            newStop.lat = coord.results[0].geometry.location.lat;
            newStop.lng = coord.results[0].geometry.location.lng;

            //some check indexing logic for stop number
            //trip.stops.legnth or .count ++ or +1
            //stop.number = the above line

            trip.Stops.Add(newStop);
            _applicationDbContext.Add(trip);
            _applicationDbContext.Add<Stop>(newStop);
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

