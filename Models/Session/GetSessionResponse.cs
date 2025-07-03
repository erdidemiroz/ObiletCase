using System.Text.Json.Serialization;

namespace ObiletCase.Models.Session
{
    public class GetSessionResponse
    {
        public SessionData Data { get; set; }
    }

    public class SessionData
    {
        [JsonPropertyName("session-id")]
        public string SessionId { get; set; }

        [JsonPropertyName("device-id")]
        public string DeviceId { get; set; }
    }
}
