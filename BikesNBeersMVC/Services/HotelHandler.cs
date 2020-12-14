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
        private readonly ICoordinateHandler _coordinateHandler;

        public HotelHandler(ICoordinateHandler coord)
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://maps.googleapis.com/maps/api/")
            };
            _coordinateHandler = coord;
        }

        public async Task<HotelResponse> GetHotel(string zipcode)
        {
            // call CoordinateHandler Service to do this
            //var httpResponseLatLong = _httpClient.GetAsync($"geocode/json?address={zipcode}&key=AIzaSyDDQ1uMLrSYDQtlX-VIFyyiXMB5_dRJNqU").GetAwaiter().GetResult();
            //httpResponseLatLong.EnsureSuccessStatusCode();
            //var contentLatLong = httpResponseLatLong.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            //var resultLatLong = JsonSerializer.Deserialize<Coordinate>(contentLatLong);

            var coordResults = await _coordinateHandler.GetCoordinates(zipcode);

            HotelResponse result = new HotelResponse();
            if (coordResults != null && coordResults.results.Length > 0 && coordResults.results[0].geometry != null)
            {
                var httpResponse = await _httpClient.GetAsync($"place/nearbysearch/json?location={coordResults.results[0].geometry.location.lat},{coordResults.results[0].geometry.location.lng}&radius=5000&keyword=hotel&key=AIzaSyAuKgJKHj3zOAMfx9bGAK8in1s4pYhl0JA");
                httpResponse.EnsureSuccessStatusCode();
                var content = await httpResponse.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<HotelResponse>(content);

                result.StartingLatitude = coordResults.results[0].geometry.location.lat;
                result.StartingLongitude = coordResults.results[0].geometry.location.lng;

                //getting photo api
                if (result != null && result.results != null)
                {
                    foreach (var hotel in result.results)
                    {
                        if (hotel.photos != null && hotel.photos.Length > 0)
                            hotel.photoURL = $"https://maps.googleapis.com/maps/api/place/photo?maxwidth=100&photoreference={hotel.photos[0].photo_reference}&key=AIzaSyAuKgJKHj3zOAMfx9bGAK8in1s4pYhl0JA";
                    }
                }
            }

            

            return result;
        }
    }
}
