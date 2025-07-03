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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Check for session and device ID in TempData
            var sessionId = TempData["session_id"] as string;
            var deviceId = TempData["device_id"] as string;

            if (string.IsNullOrEmpty(sessionId) || string.IsNullOrEmpty(deviceId))
            {
                // Create a new session if not present
                var response = await _obiletService.GetSessionAsync();
                sessionId = response.SessionId;
                deviceId = response.DeviceId;

                TempData["session_id"] = sessionId;
                TempData["device_id"] = deviceId;
            }

            var viewModel = new IndexViewModel
            {
                SessionId = sessionId,
                DepartureDate = DateTime.Today.AddDays(1) // default: tomorrow
            };

            TempData.Keep(); // preserve TempData across requests
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            // Get or refresh session info
            var sessionId = TempData["session_id"] as string;
            var deviceId = TempData["device_id"] as string;

            if (string.IsNullOrEmpty(sessionId) || string.IsNullOrEmpty(deviceId))
            {
                var response = await _obiletService.GetSessionAsync();
                sessionId = response.SessionId;
                deviceId = response.DeviceId;

                TempData["session_id"] = sessionId;
                TempData["device_id"] = deviceId;
            }

            
            //  Normally, it is more convenient and correct to manage validations with a library such as FluentValidation in the service layer,
            //  but since I am making a simple example case, I am doing quick validation checks on the controller.
            if (model.OriginId == model.DestinationId)
            {
                ModelState.AddModelError("", "Origin and destination cannot be the same.");
            }

            if (model.DepartureDate.Date < DateTime.Today)
            {
                ModelState.AddModelError("", "Departure date cannot be in the past.");
            }

            if (!ModelState.IsValid)
            {
                TempData.Keep();
                return View(model);
            }

            // Redirect to Journey page with valid data
            return RedirectToAction("Index", "Journey", new
            {
                originId = model.OriginId,
                destinationId = model.DestinationId,
                departureDate = model.DepartureDate.ToString("yyyy-MM-dd"),
                sessionId = sessionId
            });
        }

        [HttpGet]
        public async Task<IActionResult> SearchLocations(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword) || keyword.Length < 2)
                return Json(new List<object>());

            var sessionId = TempData["session_id"] as string;
            var deviceId = TempData["device_id"] as string;

            if (string.IsNullOrEmpty(sessionId) || string.IsNullOrEmpty(deviceId))
            {
                var response = await _obiletService.GetSessionAsync();
                sessionId = response.SessionId;
                deviceId = response.DeviceId;

                TempData["session_id"] = sessionId;
                TempData["device_id"] = deviceId;
            }

            var request = new GetBusLocationRequest
            {
                Data = keyword,
                Date = DateTime.UtcNow,
                Language = "en-EN",
                DeviceSession = new DeviceSession
                {
                    SessionId = sessionId,
                    DeviceId = deviceId
                }
            };

            TempData.Keep();
            var result = await _obiletService.GetBusLocationsAsync(request);
            return Json(result);
        }
    }
}
