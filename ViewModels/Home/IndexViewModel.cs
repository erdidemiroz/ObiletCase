using ObiletCase.Models.Location;

namespace ObiletCase.ViewModels.Home
{
    public class IndexViewModel
    {
        public string SessionId { get; set; }
        public List<LocationModel> AllLocations { get; set; }
        public int? OriginId { get; set; }
        public int? DestinationId { get; set; }
        public DateTime DepartureDate { get; set; }
    }
}