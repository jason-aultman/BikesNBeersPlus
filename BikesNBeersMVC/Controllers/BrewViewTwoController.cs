using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikesNBeersMVC.Models;
using BikesNBeersMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace BikesNBeersMVC.Controllers
{
  [Route("BrewViewTwo")]
  public class BrewViewTwoController : Controller
  {
    private readonly IBrewHandler _brewHandler;

    public BrewViewTwoController(IBrewHandler brew)
    {
      _brewHandler = brew;
    }
    [Route("Index")]
    public IActionResult Index(Welcome welcome)
    {
      var breweryResults = _brewHandler.GetBreweryByAddress(welcome.AddressStart, welcome.MaxMiles);
      var viewModel = new ViewModel();
      viewModel.Breweries = breweryResults;
      return View(viewModel);
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
    [Route("selectedBrewView")]
        public IActionResult selectedBrewView(ViewModel selectedBrewViewModel)
        {
          var lstSelected = selectedBrewViewModel.Breweries.Results.Where(x => x.selected == 1).ToList();
          return View(lstSelected);
        }

    //public IActionResult Index(int zip, double miles)
    //{
    //    var breweryResults = _brewHandler.GetBrewery(zip, miles);
    //    var viewModel = new ViewModel();
    //    viewModel.Breweries = breweryResults;
    //    return View();
    //}
  }
}

