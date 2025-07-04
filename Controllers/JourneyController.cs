using Microsoft.AspNetCore.Mvc;
using ObiletCase.Models.Journey;
using ObiletCase.Models.Location;
using ObiletCase.Services;
using ObiletCase.ViewModels.Home;
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
        public async Task<IActionResult> Index(int originId, int destinationId, DateTime departureDate, string originName, string destinationName)
        {
            // Retrieve session and device ID from browser cookies
            string sessionId = Request.Cookies["session_id"];
            string deviceId = Request.Cookies["device_id"];

            // Basic server-side validations to prevent invalid input
            if (originId == destinationId)
                ModelState.AddModelError("", "Origin and destination cannot be the same.");

            if (departureDate < DateTime.Today)
                ModelState.AddModelError("", "Departure date cannot be in the past.");

            // Return user to search screen if validation fails
            if (!ModelState.IsValid)
            {
                var fallbackModel = new IndexViewModel
                {
                    SessionId = sessionId,
                    DeviceId = deviceId,
                    OriginId = originId,
                    DestinationId = destinationId,
                    DepartureDate = departureDate,
                    OriginName = originName,
                    DestinationName = destinationName
                };
                return View("~/Views/Home/Index.cshtml", fallbackModel);
            }

            // Construct journey search request using provided values
            var searchRequest = new JourneySearchRequest
            {
                Data = new Data
                {
                    OriginId = originId,
                    DestinationId = destinationId,
                    DepartureDate = departureDate
                },
                Language = "en-EN",
                DeviceSession = new DeviceSession
                {
                    SessionId = sessionId,
                    DeviceId = deviceId
                }
            };

            // Query the available journeys from the API
            var journeys = await _obiletService.GetJourneysAsync(searchRequest);

            // Prepare final view model with journey results and details
            var viewModel = new JourneyIndexViewModel
            {
                Journeys = journeys,
                Origin = originName,
                Destination = destinationName,
                DepartureDate = departureDate
            };

            return View("JourneyIndex", viewModel);
        }
    }
}
