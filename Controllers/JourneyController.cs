using Microsoft.AspNetCore.Mvc;
using ObiletCase.Models.Journey;
using ObiletCase.Services;
using ObiletCase.ViewModels.Journey;

namespace ObiletCase.Controllers
{
    public class JourneyController : Controller
    {
        private readonly IObiletApiService _obiletService;

        public JourneyController(IObiletApiService obiletService)
        {
            _obiletService = obiletService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int originId, int destinationId, DateTime departureDate, string sessionId)
        {
            // Create request object from query parameters
            var searchRequest = new JourneySearchRequest
            {
                OriginId = originId,
                DestinationId = destinationId,
                DepartureDate = departureDate.ToString("yyyy-MM-dd")
            };

            // Call Obilet API service to fetch journeys
            var journeys = await _obiletService.GetJourneysAsync(searchRequest, sessionId);

            var viewModel = new JourneyIndexViewModel
            {
                Origin = originId.ToString(), // optionally get name from DB or cache
                Destination = destinationId.ToString(),
                DepartureDate = departureDate,
                Journeys = journeys
            };

            return View(viewModel);
        }
    }
}