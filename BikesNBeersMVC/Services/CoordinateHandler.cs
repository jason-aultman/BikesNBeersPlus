using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BikesNBeersMVC.Models;

namespace BikesNBeersMVC.Services
{
    public class CoordinateHandler : ICoordinateHandler
    {
        private HttpClient _httpClient;
        private JsonSerializerOptions _options;

        public CoordinateHandler()
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
        public Coordinate GetCoordinates(int zipCode)
        {

            var httpResponse = _httpClient.GetAsync($"geocode/json?address={zipCode}&key=AIzaSyDDQ1uMLrSYDQtlX-VIFyyiXMB5_dRJNqU").GetAwaiter().GetResult();
            if (httpResponse.IsSuccessStatusCode)
            {
                var content = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var coordinate = JsonSerializer.Deserialize<Coordinate>(content, _options);
                return coordinate;
            }
            else return new Coordinate();


            //var httpResponse = await client.GetAsync($"https://maps.googleapis.com/maps/api/geocode/json?address=67337&key=AIzaSyDDQ1uMLrSYDQtlX-VIFyyiXMB5_dRJNqU");
            //// https://maps.googleapis.com/maps/api/geocode/json?address=zipcode&key=YOUR_API_KEY
            //// replace zipcode with original code
            //// replace YOUR_API_KEY with the api key from google
            //httpResponse.EnsureSuccessStatusCode();
            //var content = await httpResponse.Content.ReadAsStringAsync();
            //var result = JsonConvert.DeserializeObject<Rootobject>(content);


            //InvalidOperationException: The model item passed into the ViewDataDictionary is of type 'BikesNBeerVersion2.Services.Rootobject', but this ViewDataDictionary instance requires a model item of type 'BikesNBeerVersion2.Services.Result'.

        }

        public Coordinate GetCoordinatesByAddress(string address)
        {
            var refinedAddress=address.Replace(' ', '+');
            var httpResponse = _httpClient.GetAsync($"geocode/json?address={refinedAddress}&key=AIzaSyDDQ1uMLrSYDQtlX-VIFyyiXMB5_dRJNqU").GetAwaiter().GetResult();
            if (httpResponse.IsSuccessStatusCode)
            {
                var content = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var coordinate = JsonSerializer.Deserialize<Coordinate>(content, _options);
                return coordinate;
            }
            else return new Coordinate();
        }
    }
}
