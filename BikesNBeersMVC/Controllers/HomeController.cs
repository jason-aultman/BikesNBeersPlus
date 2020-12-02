using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BikesNBeersMVC.Models;
using BikesNBeersMVC.Services;

namespace BikesNBeersMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var coordinate = new CoordinateHandler();
            var hotelResponse = new HotelHandler();
            var breweryResponse = new BrewHandler();
            var viewModel = new ViewModel();
            var testCoordinate = coordinate.GetCoordinates(90210);
            var testHotelResponse = hotelResponse.Hotel(90210);
            var testHotelResponseResult = testHotelResponse.GetAwaiter().GetResult();
            var testbreweryResponse = breweryResponse.GetBrewery(90210);
            var testbreweryResponseResult = testbreweryResponse.GetAwaiter().GetResult();
            viewModel.Breweries = testbreweryResponseResult;
            viewModel.HotelResponses = testHotelResponseResult;

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
