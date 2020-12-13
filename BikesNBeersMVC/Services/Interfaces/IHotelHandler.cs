using BikesNBeersMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesNBeersMVC.Services.Interfaces
{
    public interface IHotelHandler
    {
        Task<HotelResponse> GetHotel(string address);
    }
}
