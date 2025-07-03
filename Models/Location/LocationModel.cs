using System.Text.Json.Serialization;

namespace ObiletCase.Models.Location
{
    public class LocationModel
    {
        public int Id { get; set; }

        [JsonPropertyName("parent-id")]
        public int ParentId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }

        [JsonPropertyName("geo-location")]
        public GeoLocation GeoLocation { get; set; }

        [JsonPropertyName("tz-code")]
        public string TimeZoneCode { get; set; }

        [JsonPropertyName("weather-code")]
        public string WeatherCode { get; set; }

        public int Rank { get; set; }

        [JsonPropertyName("reference-code")]
        public string ReferenceCode { get; set; }
    }

    public class GeoLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Zoom { get; set; }
    }
}