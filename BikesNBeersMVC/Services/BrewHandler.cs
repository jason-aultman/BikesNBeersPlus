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
    public class BrewHandler : IBrewHandler
    {
        private HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        private readonly ICoordinateHandler _coordinateHandler;

        public BrewHandler(ICoordinateHandler coord)
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://maps.googleapis.com/maps/api/")
            };
            _options = new JsonSerializerOptions()
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
            };
            _coordinateHandler = coord;
        }

        public BreweryResponse GetBrewery(int zipcode)
        {
            var coordResults = _coordinateHandler.GetCoordinates(zipcode);

            BreweryResponse result = new BreweryResponse();
            if (coordResults != null && coordResults.results.Length > 0 && coordResults.results[0].geometry != null)
            {
                var httpResponse = _httpClient.GetAsync($"place/nearbysearch/json?location={coordResults.results[0].geometry.location.lat},{coordResults.results[0].geometry.location.lng}&radius=5000&keyword=brewery&key=AIzaSyAuKgJKHj3zOAMfx9bGAK8in1s4pYhl0JA").GetAwaiter().GetResult();
                httpResponse.EnsureSuccessStatusCode();
                var content = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                result = JsonSerializer.Deserialize<BreweryResponse>(content, _options);
            }
            return result;
        }

        public BreweryResponse GetBrewery(int zipcode, double distance_in_miles)
        {

          
            var coordResults = _coordinateHandler.GetCoordinates(zipcode);
            var distance_In_Meters = distance_in_miles * 1609.34;

            BreweryResponse result = new BreweryResponse();
            if (coordResults != null && coordResults.results.Length > 0 && coordResults.results[0].geometry != null)
            {
                var httpResponse = _httpClient.GetAsync($"place/nearbysearch/json?location={coordResults.results[0].geometry.location.lat},{coordResults.results[0].geometry.location.lng}&radius={distance_In_Meters}&keyword=brewery&key=AIzaSyAuKgJKHj3zOAMfx9bGAK8in1s4pYhl0JA").GetAwaiter().GetResult();
                httpResponse.EnsureSuccessStatusCode();
                var content = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                result = JsonSerializer.Deserialize<BreweryResponse>(content, _options);

                //getting photo api
                if (result != null && result.Results != null)
                {
                    foreach (var brewery in result.Results)
                    {
                        if (brewery.Photos != null && brewery.Photos.Count > 0)
                            brewery.photoURL = $"https://maps.googleapis.com/maps/api/place/photo?maxwidth=100&photoreference={brewery.Photos[0].Photo_Reference}&key=AIzaSyAuKgJKHj3zOAMfx9bGAK8in1s4pYhl0JA";
                    }
                }
            }

            return result;
        }

        public BreweryResponse GetBreweryByAddress(string address, double distance_in_miles)
        {

          
            var coordResults = _coordinateHandler.GetCoordinatesByAddress(address);
            var distance_In_Meters = distance_in_miles * 1609.34;

            BreweryResponse result = new BreweryResponse();
            if (coordResults != null && coordResults.results.Length > 0 && coordResults.results[0].geometry != null)
            {
                var httpResponse = _httpClient.GetAsync($"place/nearbysearch/json?location={coordResults.results[0].geometry.location.lat},{coordResults.results[0].geometry.location.lng}&radius={distance_In_Meters}&keyword=brewery&key=AIzaSyAuKgJKHj3zOAMfx9bGAK8in1s4pYhl0JA").GetAwaiter().GetResult();
                httpResponse.EnsureSuccessStatusCode();
                var content = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                result = JsonSerializer.Deserialize<BreweryResponse>(content, _options);

                //getting photo api
                if (result != null && result.Results != null)
                {
                    foreach (var brewery in result.Results)
                    {
                        if (brewery.Photos != null && brewery.Photos.Count > 0)
                            brewery.photoURL = $"https://maps.googleapis.com/maps/api/place/photo?maxwidth=100&photoreference={brewery.Photos[0].Photo_Reference}&key=AIzaSyAuKgJKHj3zOAMfx9bGAK8in1s4pYhl0JA";
                    }
                }
            }

            return result;
        }
    }
}
