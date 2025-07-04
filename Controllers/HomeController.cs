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
            // Retrieve session and device ID from browser cookies
            string sessionId = Request.Cookies["session_id"];
            string deviceId = Request.Cookies["device_id"];

            // If cookie not available, call GetSession API and store values in cookies
            if (string.IsNullOrEmpty(sessionId) || string.IsNullOrEmpty(deviceId))
            {
                var response = await _obiletService.GetSessionAsync();
                sessionId = response.SessionId;
                deviceId = response.DeviceId;

                // Save values in browser cookies for subsequent requests
                Response.Cookies.Append("session_id", sessionId, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });

                Response.Cookies.Append("device_id", deviceId, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });
            }

            // Prepare the default view model for the search screen
            var viewModel = new IndexViewModel
            {
                SessionId = sessionId,
                DeviceId = deviceId,
                DepartureDate = DateTime.Today.AddDays(1) // default: tomorrow
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            // Retrieve session/device ID from cookies again for safety
            string sessionId = Request.Cookies["session_id"];
            string deviceId = Request.Cookies["device_id"];

            // Pass session and device IDs to view model
            model.SessionId = sessionId;
            model.DeviceId = deviceId;

            // Add Origin and Destination Name to TempData
            TempData["origin_name"] = model.OriginName ?? "";
            TempData["destination_name"] = model.DestinationName ?? "";

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

            // Redirect to JourneyController with validated query parameters
            return RedirectToAction("Index", "Journey", new
            {
                originId = model.OriginId,
                destinationId = model.DestinationId,
                departureDate = model.DepartureDate.ToString("yyyy-MM-dd"),
                sessionId = model.SessionId,
                deviceId = model.DeviceId,
                originName = model.OriginName,
                destinationName = model.DestinationName
            });
        }

        [HttpGet]
        public async Task<IActionResult> SearchLocations(string keyword)
        {
            // Minimum keyword length for search
            if (string.IsNullOrWhiteSpace(keyword) || keyword.Length < 2)
                return Json(new List<object>());

            // Retrieve session/device ID from cookies again
            string sessionId = Request.Cookies["session_id"];
            string deviceId = Request.Cookies["device_id"];


            // If session/device ID not available, create new session and update cookies
            if (string.IsNullOrEmpty(sessionId) || string.IsNullOrEmpty(deviceId))
            {
                var response = await _obiletService.GetSessionAsync();
                sessionId = response.SessionId;
                deviceId = response.DeviceId;

                Response.Cookies.Append("session_id", sessionId, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });

                Response.Cookies.Append("device_id", deviceId, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });
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