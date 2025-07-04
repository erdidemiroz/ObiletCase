using System.Text.Json.Serialization;
using ObiletCase.Models.Location;

namespace ObiletCase.Models.Journey;

public class JourneySearchRequest
{
    public Data Data { get; set; }
    public string Language { get; set; }

    [JsonPropertyName("device-session")]
    public DeviceSession DeviceSession { get; set; }
}

public class Data
{
    [JsonPropertyName("origin-id")]
    public int OriginId { get; set; }

    [JsonPropertyName("destination-id")]
    public int DestinationId { get; set; }

    [JsonPropertyName("departure-date")]
    public DateTime DepartureDate { get; set; }
}