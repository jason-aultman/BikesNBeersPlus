using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using BikesNBeersMVC.Models;
using System.Text.Json;

namespace BikesNBeersMVC.Services
{
    public class HotelHandler
    {
        private HttpClient _httpClient;

        public HotelHandler()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://maps.googleapis.com/maps/api/")
            };
        }

        public async Task<HotelResponse> Hotel(int zipcode)
        {
            // call CoordinateHandler Service to do this
            var httpResponseLatLong = await _httpClient.GetAsync($"geocode/json?address={zipcode}&key=AIzaSyDDQ1uMLrSYDQtlX-VIFyyiXMB5_dRJNqU");
            httpResponseLatLong.EnsureSuccessStatusCode();
            var contentLatLong = await httpResponseLatLong.Content.ReadAsStringAsync();
            var resultLatLong = JsonSerializer.Deserialize<Coordinate>(contentLatLong);


            HotelResponse result = new HotelResponse();
            if (resultLatLong != null && resultLatLong.results.Length > 0 && resultLatLong.results[0].geometry != null)
            {
                //string url = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={resultLatLong.results[0].geometry.location.lat}.1298305&radius=1500&type=lodging&keyword=hotels&key=%20AIzaSyAuKgJKHj3zOAMfx9bGAK8in1s4pYhl0JA";
                var httpResponse = await _httpClient.GetAsync($"place/nearbysearch/json?location={resultLatLong.results[0].geometry.location.lat},{resultLatLong.results[0].geometry.location.lng}&radius=5000&keyword=hotel&key=AIzaSyAuKgJKHj3zOAMfx9bGAK8in1s4pYhl0JA");
                httpResponse.EnsureSuccessStatusCode();
                var content = await httpResponse.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<HotelResponse>(content);
            }

            //ViewBag["result"] = result;

            return result;
        }
    }
}
