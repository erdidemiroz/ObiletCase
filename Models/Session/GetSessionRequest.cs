using System.Text.Json.Serialization;

namespace ObiletCase.Models.Session
{
    public class GetSessionRequest
    {
        public string Type { get; set; }
        public Connection Connection { get; set; }
        public Application Application { get; set; }
    }

    public class Connection
    {
        [JsonPropertyName("ip-address")]
        public string IpAddress { get; set; }
    }

    public class Application
    {
        [JsonPropertyName("version")]
        public string Version { get; set; }


        [JsonPropertyName("equipment-id")]
        public string EquipmentId { get; set; }
    }
}
