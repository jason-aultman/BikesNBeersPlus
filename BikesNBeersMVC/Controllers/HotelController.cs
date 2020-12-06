using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BikesNBeersMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static BikesNBeersMVC.Models.Geocode;

namespace BikesNBeersMVC.Controllers
{
    [Route("hotel")]
    public class HotelController : Controller
    {
        public IActionResult Hotel()
        {
            return View("index");
        }

        [HttpPost]
        public async Task<IActionResult> Hotel(HotelSearchModel model)
        {

            var client = new HttpClient();
            var httpResponseLatLong = await client.GetAsync($"https://maps.googleapis.com/maps/api/geocode/json?address={model.ZipCode}&key=AIzaSyDDQ1uMLrSYDQtlX-VIFyyiXMB5_dRJNqU");
            httpResponseLatLong.EnsureSuccessStatusCode();
            var contentLatLong = await httpResponseLatLong.Content.ReadAsStringAsync();
            var resultLatLong = JsonConvert.DeserializeObject<Rootobject>(contentLatLong);


            HotelResponse result = new HotelResponse();
            if (resultLatLong != null && resultLatLong.results.Length > 0 && resultLatLong.results[0].geometry != null)
            {
                //string url = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={resultLatLong.results[0].geometry.location.lat}.1298305&radius=1500&type=lodging&keyword=hotels&key=%20AIzaSyAuKgJKHj3zOAMfx9bGAK8in1s4pYhl0JA";
                var httpResponse = await client.GetAsync($"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={resultLatLong.results[0].geometry.location.lat},{resultLatLong.results[0].geometry.location.lng}&radius=5000&keyword=hotel&key=AIzaSyAuKgJKHj3zOAMfx9bGAK8in1s4pYhl0JA");
                httpResponse.EnsureSuccessStatusCode();
                var content = await httpResponse.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<HotelResponse>(content);
            }



            

            return View(result.results);
        }


    }
}

