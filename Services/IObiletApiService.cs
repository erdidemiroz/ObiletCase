using ObiletCase.Models.Journey;
using ObiletCase.Models.Location;
using ObiletCase.Models.Session;

namespace ObiletCase.Services
{
    public interface IObiletApiService
    {
        Task<SessionData> GetSessionAsync();
        Task<List<LocationModel>> GetBusLocationsAsync(GetBusLocationRequest request);
        Task<List<JourneyModel>> GetJourneysAsync(JourneySearchRequest request);
    }
}
