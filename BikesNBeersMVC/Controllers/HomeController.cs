using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BikesNBeersMVC.Models;
using BikesNBeersMVC.Services;
using Microsoft.AspNetCore.Authorization;

namespace BikesNBeersMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICoordinateHandler _coordinateHandler;
       
        public HomeController(ILogger<HomeController> logger, ICoordinateHandler coordinate)
        {
            _logger = logger;
            _coordinateHandler = coordinate;
        }
       // [Authorize]
        public async Task<IActionResult> Index()
        {
          //  var coordinate = new CoordinateHandler();
            var hotelResponse = new HotelHandler(_coordinateHandler);
            var breweryResponse = new BrewHandler(_coordinateHandler);
            var Coordinate1 = new Coordinate();
            var Coordinate2 = new Coordinate();
            Coordinate1.results = new BikesNBeersMVC.Models.Result[1];
            Coordinate2.results = new BikesNBeersMVC.Models.Result[1];

            Coordinate1.results[0] = new Models.Result();
            Coordinate2.results[0] = new Models.Result();

            Coordinate1.results[0].geometry = new Models.Geometry();
            Coordinate2.results[0].geometry = new Models.Geometry();

            Coordinate1.results[0].geometry.location = new Models.Location();
            Coordinate2.results[0].geometry.location = new Models.Location();



            Coordinate1.results[0].geometry.location.lat = 42.4806F;
            Coordinate1.results[0].geometry.location.lng = -83.47555F;
            Coordinate2.results[0].geometry.location.lat = 42.3684F;
            Coordinate2.results[0].geometry.location.lng = -83.3527F;
          
            var testCoordinate = await _coordinateHandler.GetCoordinates("90210");
            var testHotelResponse = await hotelResponse.GetHotel("90210");
            var testHotelResponseResult = testHotelResponse;
            var testbreweryResponse = await breweryResponse.GetBrewery("90210");
            var testbreweryResponseResult = testbreweryResponse;

            //broke this
            return View();
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
