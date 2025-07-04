using ObiletCase.Models.Location;

namespace ObiletCase.ViewModels.Home
{
    public class IndexViewModel
    {
        public string SessionId { get; set; }
        public string DeviceId { get; set; }
        public List<LocationModel> AllLocations { get; set; } = new List<LocationModel>();
        public int? OriginId { get; set; }
        public int? DestinationId { get; set; }
        public string OriginName { get; set; }
        public string DestinationName { get; set; }
        public DateTime DepartureDate { get; set; }
    }
}