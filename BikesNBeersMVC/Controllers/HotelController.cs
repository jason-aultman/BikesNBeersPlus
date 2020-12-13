using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BikesNBeersMVC.Context;
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
    private readonly ApplicationDbContext _applicationDbContext;

    public HotelController(ICoordinateHandler coordinate, ApplicationDbContext applicationDbContext)
    {
      _coordinateHandler = coordinate;
      _applicationDbContext = applicationDbContext;

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
    public async Task<IActionResult> selectHotels(List<HotelResult> hotels)
    {
      var lstSelected = hotels.Where(x => x.selected == 1).ToList();
            var newStop = new Stop() { IsHotel = true, Name = lstSelected[0].name, Address = lstSelected[0].vicinity, Phone = " ", Photo = lstSelected[0].photoURL, StopOrderNumber = 1 };
           var coord = _coordinateHandler.GetCoordinatesByAddress(lstSelected[0].vicinity);
            newStop.lat = coord.results[0].geometry.location.lat;
            newStop.lng = coord.results[0].geometry.location.lng;

            _applicationDbContext.Add<Stop>(newStop);
           await _applicationDbContext.SaveChangesAsync();
      return View(lstSelected);
    }


  }
}

