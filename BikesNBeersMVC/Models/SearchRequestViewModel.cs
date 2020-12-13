using System;
namespace BikesNBeersMVC.Models
{
    public class SearchRequestViewModel
    {
        public SearchRequestViewModel()
        {
        }

        //can be zip code or full address
        public string Query { get; set; }
        //can be hotel or brewery
        public string StopType { get; set; }
    }
}
