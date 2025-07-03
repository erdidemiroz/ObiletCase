using Microsoft.AspNetCore.Mvc;
using ObiletCase.Models.Location;
using ObiletCase.Services;
using ObiletCase.ViewModels.Home;

namespace ObiletCase.Controllers
{
    public class HomeController : Controller
    {
        private readonly IObiletApiService _obiletService;

        public HomeController(IObiletApiService obiletService)
        {
            _obiletService = obiletService;
        }

        public async Task<IActionResult> Index()
        {
            // Check if session exists in TempData, if not create new one
            var sessionId = TempData["session_id"] as string;
            var deviceId = TempData["device_id"] as string;

            if (string.IsNullOrEmpty(sessionId))
            {
                var response = await _obiletService.GetSessionAsync();
                sessionId = response.SessionId;
                deviceId = response.DeviceId;

                // Store sessionId, deviceId in TempData to persist across requests
                TempData["session_id"] = sessionId;
                TempData["device_id"] = deviceId;
            }

            var request = new GetBusLocationRequest
            {
                Data = null,
                DeviceSession = new DeviceSession
                {
                    SessionId = sessionId,
                    DeviceId = deviceId
                },
                Date = DateTime.UtcNow
            };

            var locations = await _obiletService.GetBusLocationsAsync(request);
            var viewModel = new IndexViewModel
            {
                SessionId = sessionId,
                AllLocations = locations,
                DepartureDate = DateTime.Today.AddDays(1) // default: tomorrow
            };

            return View(viewModel);
        }
    }
}