using Microsoft.AspNetCore.Mvc;
using ObiletCase.Models.Journey;
using ObiletCase.Models.Location;
using ObiletCase.Services;
using ObiletCase.ViewModels.Home;
using ObiletCase.ViewModels.Journey;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Index(int originId, int destinationId, DateTime departureDate, string sessionId, string deviceId)
        {
            // Basic validations
            if (originId == destinationId)
                ModelState.AddModelError("", "Origin and destination cannot be the same.");

            if (departureDate < DateTime.Today)
                ModelState.AddModelError("", "Departure date cannot be in the past.");

            // Return to Home/Index view if validation fails
            if (!ModelState.IsValid)
            {
                var fallbackModel = new IndexViewModel
                {
                    SessionId = sessionId,
                    DeviceId = deviceId,
                    OriginId = originId,
                    DestinationId = destinationId,
                    DepartureDate = departureDate,
                    AllLocations = TempData["locations"] as List<LocationModel> ?? new List<LocationModel>()
                };

                TempData.Keep("locations");

                return View("~/Views/Home/Index.cshtml", fallbackModel);
            }

            // Build request model
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

            // Call the service
            var journeys = await _obiletService.GetJourneysAsync(searchRequest);

            // Get names from TempData
            var originName = TempData["origin_name"]?.ToString() ?? "";
            var destinationName = TempData["destination_name"]?.ToString() ?? "";

            // Create view model
            var viewModel = new JourneyIndexViewModel
            {
                Journeys = journeys,
                Origin = originName,
                Destination = destinationName,
                DepartureDate = departureDate
            };

            // Return JourneyIndex view
            return View("JourneyIndex", viewModel);
        }
    }
}
