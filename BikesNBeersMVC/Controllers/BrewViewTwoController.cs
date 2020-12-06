using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikesNBeersMVC.Models;
using BikesNBeersMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace BikesNBeersMVC.Controllers
{
    public class BrewViewTwoController : Controller
    {
        private readonly IBrewHandler _brewHandler;
       
        public BrewViewTwoController(IBrewHandler brew)
        {
            _brewHandler = brew;
        }
        public IActionResult Index(Welcome welcome)
        {
            var breweryResults = _brewHandler.GetBrewery(welcome.ZipCodeStart, welcome.MaxMiles);
            var viewModel = new ViewModel();
            viewModel.Breweries = breweryResults;
            return View();
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