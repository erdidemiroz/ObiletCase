using ObiletCase.Models;
using ObiletCase.Models.Journey;
using ObiletCase.Models.Location;
using ObiletCase.Models.Session;

namespace ObiletCase.Services
{
    public class ObiletApiService : IObiletApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _token = "JEcYcEMyantZV095WVc3G2JtVjNZbWx1";

        public ObiletApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://v2-api.obilet.com/api/");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {_token}");
        }

        // Create session for each user (called once per user)
        public async Task<SessionData> GetSessionAsync()
        {
            var request = new GetSessionRequest
            {
                Type = "2",
                Connection = new Connection
                {
                    IpAddress = "145.214.41.21"
                },
                Application = new Application
                {
                    EquipmentId = "distribusion",
                    Version = "1.0.0.0"
                }
            };
            var response = await _httpClient.PostAsJsonAsync("client/getsession", request);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to create session");

            var result = await response.Content.ReadFromJsonAsync<BaseResponse<SessionData>>();

            return result?.Data ?? new SessionData();
        }

        // Fetch bus locations optionally filtered by keyword
        public async Task<List<LocationModel>> GetBusLocationsAsync(GetBusLocationRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("location/getbuslocations", request);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to fetch locations");

            var result = await response.Content.ReadFromJsonAsync<BaseResponse<List<LocationModel>>>();

            return result?.Data ?? new List<LocationModel>();
        }

        public async Task<List<JourneyModel>> GetJourneysAsync(JourneySearchRequest request, string sessionId)
        {
            var requestBody = new
            {
                data = new
                {
                    origin_id = request.OriginId,
                    destination_id = request.DestinationId,
                    departure_date = request.DepartureDate
                },
                device_session = new
                {
                    session_id = sessionId,
                    device_id = "test-device-id"
                },
                date = DateTime.UtcNow
            };

            var response = await _httpClient.PostAsJsonAsync("journey/getbusjourneys", requestBody);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to fetch journeys");

            var result = await response.Content.ReadFromJsonAsync<BaseResponse<List<JourneyModel>>>();

            return result?.Data.OrderBy(j => j.Departure).ToList() ?? new();
        }
    }
}
