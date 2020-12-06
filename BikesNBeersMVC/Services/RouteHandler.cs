using BikesNBeersMVC.Models;
using BikesNBeersMVC.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BikesNBeersMVC.Services
{
    public class RouteHandler : IRouteHandler
    {
        private readonly JsonSerializerOptions _options;
        private HttpClient _httpClient;
        public RouteHandler()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://maps.googleapis.com/maps/api/")

            };

            _options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

        }
        public RouteModel GetRoute(Coordinate Start, Coordinate End)
        {
            var directionApiRequest = _httpClient.GetAsync($"directions/json?origin={Start.results[0].geometry.location.lat},{Start.results[0].geometry.location.lng}&destination={End.results[0].geometry.location.lat},{End.results[0].geometry.location.lng}&mode=bicycling&key=AIzaSyCKzsJhKiiicui9B1qNQKHO85JdbHzizIo").GetAwaiter().GetResult();
            var requestToJson = directionApiRequest.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var routeModel = JsonSerializer.Deserialize<RouteModel>(requestToJson, _options);
            return routeModel;
        }
  
    }

}



