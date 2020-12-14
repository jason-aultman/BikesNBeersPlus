using System;
namespace BikesNBeersMVC
{
    public class Settings
    {
        public Settings(string apiKey)
        {
            ApiKey = apiKey;   
        }
        public string ApiKey { get; set; }
    }
}
