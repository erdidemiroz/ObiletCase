using ObiletCase.Models.Journey;

namespace ObiletCase.ViewModels.Journey
{
    public class JourneyIndexViewModel
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureDate { get; set; }
        public List<JourneyModel> Journeys { get; set; } = new();
    }
}