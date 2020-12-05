using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using BikesNBeersMVC.Models;
using System.Text.Json;
using BikesNBeersMVC.Services.Interfaces;

namespace BikesNBeersMVC.Services
{
    public class HotelHandler : IHotelHandler
    {
        private HttpClient _httpClient;

        public HotelHandler()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://maps.googleapis.com/maps/api/")
            };
        }

        public HotelResponse GetHotel(int zipcode)
        {
            // call CoordinateHandler Service to do this
            var httpResponseLatLong = _httpClient.GetAsync($"geocode/json?address={zipcode}&key=AIzaSyDDQ1uMLrSYDQtlX-VIFyyiXMB5_dRJNqU").GetAwaiter().GetResult();
            httpResponseLatLong.EnsureSuccessStatusCode();
            var contentLatLong = httpResponseLatLong.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var resultLatLong = JsonSerializer.Deserialize<Coordinate>(contentLatLong);


            HotelResponse result = new HotelResponse();
            if (resultLatLong != null && resultLatLong.results.Length > 0 && resultLatLong.results[0].geometry != null)
            {
                var httpResponse = _httpClient.GetAsync($"place/nearbysearch/json?location={resultLatLong.results[0].geometry.location.lat},{resultLatLong.results[0].geometry.location.lng}&radius=5000&keyword=hotel&key=AIzaSyAuKgJKHj3zOAMfx9bGAK8in1s4pYhl0JA").GetAwaiter().GetResult();
                httpResponse.EnsureSuccessStatusCode();
                var content =  httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                result = JsonSerializer.Deserialize<HotelResponse>(content);
            }

            //ViewBag["result"] = result;

            return result;
        }
    }
}
