using BikesNBeersMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesNBeersMVC.Services.Interfaces
{
    public interface IRouteHandler
    {
        public RouteModel GetRoute(Coordinate Start, Coordinate End);

    }
}
