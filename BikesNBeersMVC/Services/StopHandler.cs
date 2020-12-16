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
    public class StopHandler : IStopHandler
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        private readonly Settings _settings;

        public StopHandler(Settings settings, HttpClient httpClient, JsonSerializerOptions options)
        {
            _settings = settings;
            _httpClient = httpClient;
            _options = options;
            
        }

        public async Task<Coordinate> GetCoordinates(string zipCode)
        {

            var httpResponse = await _httpClient.GetAsync($"geocode/json?address={zipCode}&key={_settings.ApiKey}");
            if (httpResponse.IsSuccessStatusCode)
            {
                var content = await httpResponse.Content.ReadAsStringAsync();
                var coordinate = JsonSerializer.Deserialize<Coordinate>(content, _options);
                return coordinate;
            }
            else return new Coordinate();
 
        }

        public async Task<Coordinate> GetCoordinatesByAddress(string address)
        {
            var refinedAddress = address.Replace(' ', '+');
            var httpResponse = await _httpClient.GetAsync($"geocode/json?address={refinedAddress}&key={_settings.ApiKey}");
            if (httpResponse.IsSuccessStatusCode)
            {
                var content = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var coordinate = JsonSerializer.Deserialize<Coordinate>(content, _options);
                return coordinate;
            }
            else return new Coordinate();
        }

        public async Task<List<Stop>> GetStopByAddress(string address, double distance_in_miles = 0, string type = "brewery")
        {
            var maxMiles = 0d;
            if (type == "brewery")
            {
                maxMiles = 25d;
            }
            else if (type == "hotel")
            {
                maxMiles = 2d;
            }

            if (distance_in_miles != 0)
            {
                maxMiles = distance_in_miles;
            }

            var coordResults = await GetCoordinatesByAddress(address);
            var distance_In_Meters = maxMiles * 1609.34;

            StopResponse result = new StopResponse();
            if (coordResults != null && coordResults.results.Length > 0 && coordResults.results[0].geometry != null)
            {
                var httpResponse = await _httpClient.GetAsync($"place/nearbysearch/json?location={coordResults.results[0].geometry.location.lat},{coordResults.results[0].geometry.location.lng}&radius={distance_In_Meters}&keyword={type}&key={_settings.ApiKey}");
                httpResponse.EnsureSuccessStatusCode();
                var content = await httpResponse.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<StopResponse>(content, _options);

                result.StartingLatitude = coordResults.results[0].geometry.location.lat;
                result.StartingLongitude = coordResults.results[0].geometry.location.lng;

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
            List<Stop> stops = new List<Stop>();
            foreach (var stop in result?.Results)
            {
                stops.Add(new Stop
                {
                    Name = stop.Name,
                    Rating = stop.Rating,
                    Address = stop.Vicinity,
                    Photo = stop.photoURL,
                    IsHotel = false,
                    //starting lat and starting long are based on the user search not based on the last stop lat or long
                    StartingLatitiude = result.StartingLatitude,
                    StartingLongitude = result.StartingLongitude,
                    lat = stop.Geometry.Location.Lat,
                    lng = stop.Geometry.Location.Lng,
                });
            }

            return stops;
        }

        public async Task<double> GetDistance(double startingLong, double startingLat, double endingLong, double endingLat)
        {
            var httpResponse = await _httpClient.GetAsync($"distancematrix/json?origins={startingLat},{startingLong}&destinations={endingLat},{endingLong}&travel_mode=bicycling&units=imperial&key={_settings.ApiKey}");
            if (!httpResponse.IsSuccessStatusCode)
            {
               //do some error handling for unsucessful call
            }
            var content = await httpResponse.Content.ReadAsStringAsync();
            var distanceResponce = JsonSerializer.Deserialize<DistanceResponse>(content, _options);
            return distanceResponce.rows[0].elements[0].distance.value;
        }
    }
}
