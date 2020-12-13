using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikesNBeersMVC.Models;
using BikesNBeersMVC.Services;
using BikesNBeersMVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BikesNBeersMVC.Controllers
{
    public class MapViewController : Controller
    {
        public readonly ICoordinateHandler _coordinateHandler;
        public readonly IRouteHandler _routeHandler;

        public MapViewController(ICoordinateHandler coordinateHandler, IRouteHandler routeHandler)
        {
            _coordinateHandler = coordinateHandler;
            _routeHandler = routeHandler;
        }
        public async Task<IActionResult> Index(Trip route)
        {
            var testPlace1 = new Trip();
            var testPlace2 = new Trip();
            testPlace1.Id = 0;
            testPlace1.BikerInfoId = 0;
            testPlace1.Stops.Add(new Stop() { Id = 0, Address = "2611 William Ave, Holland, MI 49424" });
            testPlace2.Id = 1;
            testPlace2.BikerInfoId = 1;
            testPlace2.Stops.Add(new Stop() { Id = 0, Address = "528 South Brooks Rd, Muskegon, MI 49442" });

            //test
            var place1 = testPlace1.Stops[0].Address;
            var place2 = testPlace2.Stops[0].Address;
            var mapViewModel = new MapViewModel();
            var start = await _coordinateHandler.GetCoordinatesByAddress(place1);
            var end = await _coordinateHandler.GetCoordinatesByAddress(place2);
            mapViewModel.StartLat = start.results[0].geometry.location.lat;
            mapViewModel.StartLng = start.results[0].geometry.location.lng;
            mapViewModel.EndLat = end.results[0].geometry.location.lat;
            mapViewModel.EndLng = end.results[0].geometry.location.lng;
            //var route = _routeHandler.GetRoute(start, end);
            return View(mapViewModel);
        }
    }
}