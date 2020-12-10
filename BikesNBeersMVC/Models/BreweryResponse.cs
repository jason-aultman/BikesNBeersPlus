using System;
using System.Collections.Generic;

namespace BikesNBeersMVC
{

    public partial class BreweryResponse
    {
        public List<object> HtmlAttributions { get; set; }
        public List<Result> Results { get; set; }
        public string Status { get; set; }
    }

    public partial class Result
    {
        public BusinessStatus BusinessStatus { get; set; }
        public Geometry Geometry { get; set; }
        public Uri Icon { get; set; }
        public string Name { get; set; }
        public OpeningHours OpeningHours { get; set; }
        public List<Photo> Photos { get; set; }
        public string PlaceId { get; set; }
        public PlusCode PlusCode { get; set; }
        public long? PriceLevel { get; set; }
        public double Rating { get; set; }
        public string Reference { get; set; }
        public string Scope { get; set; }
        public List<string> Types { get; set; }
        public long UserRatingsTotal { get; set; }
        public string Vicinity { get; set; }
        public string photoURL { get; internal set; }
    }

    public partial class Geometry
    {
        public Location Location { get; set; }
        public Viewport Viewport { get; set; }
    }

    public partial class Location
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public partial class Viewport
    {
        public Location Northeast { get; set; }
        public Location Southwest { get; set; }
    }

    public partial class OpeningHours
    {
        public bool OpenNow { get; set; }
    }

    public partial class Photo
    {
        public long Height { get; set; }
        public List<string> Html_Attributions { get; set; }
        public string Photo_Reference { get; set; }
        public long Width { get; set; }
    }

    public partial class PlusCode
    {
        public string CompoundCode { get; set; }
        public string GlobalCode { get; set; }
    }

    public enum BusinessStatus { Operational };

    public enum Scope { Google };

    public enum TypeElement { Bar, Establishment, Food, LiquorStore, PointOfInterest, Restaurant, Store };
}
