//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace BikesNBeersMVC.Services
//{
//    public class IDistanceHandler : IDistanceHandler
//    {
       
//            private HttpClient _httpClient;
//            private JsonSerializerOptions _options;

//            public DistanceHandler()
//            {
//                _httpClient = new HttpClient()
//                {
//                    BaseAddress = new Uri("https://maps.googleapis.com/maps/api/")
//                };
//                _options = new JsonSerializerOptions()
//                {
//                    PropertyNameCaseInsensitive = true
//                };

//            }
//            public async Task<double> GetDistance(double startingLong, double startingLat, double endingLong, double endingLat)
//            {

//                var httpResponse = await _httpClient.GetAsync($"geocode/json?address={zipCode}&key=AIzaSyDDQ1uMLrSYDQtlX-VIFyyiXMB5_dRJNqU");
//                if (httpResponse.IsSuccessStatusCode)
//                {
//                    var content = await httpResponse.Content.ReadAsStringAsync();
//                    var coordinate = JsonSerializer.Deserialize<Coordinate>(content, _options);
//                    return coordinate;
//                }
//                else return new Coordinate();
//            }
//}
