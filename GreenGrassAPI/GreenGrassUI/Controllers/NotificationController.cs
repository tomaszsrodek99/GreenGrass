using GreenGrassAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace GreenGrassUI.Controllers
{
    public class NotificationController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public NotificationController(IWebHostEnvironment webHostEnvironment)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44304/");
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNotification(NotificationDto notificationDto)
        {
            try
            {
                HttpResponseMessage notification = await _httpClient.PostAsJsonAsync($"api/Notification/PostNotification", notificationDto);
                if (notification.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetPlants", "Plant");
                }
                else
                {
                    throw new Exception("Błąd serwera. Spróbuj ponownie później.");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public async Task<IActionResult> UpdateWatering(int id)
        {
            try
            {
                HttpResponseMessage notification = await _httpClient.PostAsJsonAsync($"api/Plant/UpdateWateringStatus", id);
                if (notification.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetPlants", "Plant");
                }
                else
                {
                    throw new Exception("Błąd serwera. Spróbuj ponownie później.");
                }
                
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }

        }
        [Authorize]
        public async Task<IActionResult> UpdateFertilizing(int id)
        {
            try
            {
                HttpResponseMessage notification = await _httpClient.PostAsJsonAsync($"api/Plant/UpdateFertilizingStatus", id);
                if (notification.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetPlants", "Plant");
                }
                else
                {
                    throw new Exception("Błąd serwera. Spróbuj ponownie później.");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }

        }
    }
}
