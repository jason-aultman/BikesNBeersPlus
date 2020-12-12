using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BikesNBeersMVC.Models;
using BikesNBeersMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static BikesNBeersMVC.Models.Geocode;

namespace BikesNBeersMVC.Controllers
{
  [Route("hotel")]
  public class HotelController : Controller
  {
    private readonly ICoordinateHandler _coordinateHandler;
    public HotelController(ICoordinateHandler coordinate)
    {
      _coordinateHandler = coordinate;
    }
    public IActionResult Hotel()
    {
      return View("index");
    }

    [HttpPost]
    public IActionResult Hotel(HotelSearchModel model)
    {
      //creating the instance of the handler
      var hotelResponse = new HotelHandler(_coordinateHandler);
      var result = hotelResponse.GetHotel(model.Address);

      return View(result.results);
    }

    [HttpPost]
    [Route("selectHotels")]
    public IActionResult selectHotels(List<HotelResult> hotels)
    {
      var lstSelected = hotels.Where(x => x.selected == 1).ToList();
      return View(lstSelected);
    }


  }
}

