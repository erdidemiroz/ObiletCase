using System.Text.Json.Serialization;

namespace ObiletCase.Models.Location
{
    public class GetBusLocationRequest
    {
        public string Data { get; set; }
        public DateTime Date { get; set; }
        public string Language { get; set; }

        [JsonPropertyName("device-session")]
        public DeviceSession DeviceSession { get; set; }
    }
}
