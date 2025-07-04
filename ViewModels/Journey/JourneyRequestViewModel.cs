namespace ObiletCase.ViewModels.Journey
{
    public class JourneyRequestViewModel
    {
        public int OriginId { get; set; }
        public int DestinationId { get; set; }
        public DateTime DepartureDate { get; set; }
        public string SessionId { get; set; }
        public string DeviceId { get; set; }

    }
}