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

        public BrewHandler()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://maps.googleapis.com/maps/api/")
            };
            _options = new JsonSerializerOptions()
            {
                AllowTrailingCommas=true,
                PropertyNameCaseInsensitive=true,
                
                
            };
        }

        public BreweryResponse GetBrewery(int zipcode)
        {
            // call CoordinateHandler Service to do this
            var httpResponseLatLong = _httpClient.GetAsync($"geocode/json?address={zipcode}&key=AIzaSyDDQ1uMLrSYDQtlX-VIFyyiXMB5_dRJNqU").GetAwaiter().GetResult();
            httpResponseLatLong.EnsureSuccessStatusCode();
            var contentLatLong = httpResponseLatLong.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var resultLatLong = JsonSerializer.Deserialize<Coordinate>(contentLatLong);


            BreweryResponse result = new BreweryResponse();
            if (resultLatLong != null && resultLatLong.results.Length > 0 && resultLatLong.results[0].geometry != null)
            {
                var httpResponse = _httpClient.GetAsync($"place/nearbysearch/json?location={resultLatLong.results[0].geometry.location.lat},{resultLatLong.results[0].geometry.location.lng}&radius=5000&keyword=brewery&key=AIzaSyAuKgJKHj3zOAMfx9bGAK8in1s4pYhl0JA").GetAwaiter().GetResult();
                httpResponse.EnsureSuccessStatusCode();
                var content = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                result = JsonSerializer.Deserialize<BreweryResponse>(content, _options);
            }
            return result;
        }

        public BreweryResponse GetBrewery(int zipcode, int distance_in_miles)
        {
            var httpResponseLatLong = _httpClient.GetAsync($"geocode/json?address={zipcode}&key=AIzaSyDDQ1uMLrSYDQtlX-VIFyyiXMB5_dRJNqU").GetAwaiter().GetResult();
            httpResponseLatLong.EnsureSuccessStatusCode();
            var contentLatLong = httpResponseLatLong.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var resultLatLong = JsonSerializer.Deserialize<Coordinate>(contentLatLong);
            var distance_In_Meters = distance_in_miles * 1609.34;

            BreweryResponse result = new BreweryResponse();
            if (resultLatLong != null && resultLatLong.results.Length > 0 && resultLatLong.results[0].geometry != null)
            {
                var httpResponse = _httpClient.GetAsync($"place/nearbysearch/json?location={resultLatLong.results[0].geometry.location.lat},{resultLatLong.results[0].geometry.location.lng}&radius={distance_In_Meters}&keyword=brewery&key=AIzaSyAuKgJKHj3zOAMfx9bGAK8in1s4pYhl0JA").GetAwaiter().GetResult();
                httpResponse.EnsureSuccessStatusCode();
                var content = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                result = JsonSerializer.Deserialize<BreweryResponse>(content, _options);
            }

            return result;

        }
    }
}
