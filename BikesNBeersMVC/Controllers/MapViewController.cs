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
        public IActionResult Index()
        {
            //test
            var mapViewModel = new MapViewModel();
            var start = _coordinateHandler.GetCoordinates(49442);
            var end = _coordinateHandler.GetCoordinates(49424);
            mapViewModel.StartLat = start.results[0].geometry.location.lat;
            mapViewModel.StartLng = start.results[0].geometry.location.lng;
            mapViewModel.EndLat = end.results[0].geometry.location.lat;
            mapViewModel.EndLng = end.results[0].geometry.location.lng;
            //var route = _routeHandler.GetRoute(start, end);
            return View(mapViewModel);
        }
    }
}