using GreenGrassAPI.Dtos;
using GreenGrassAPI.Models;
using GreenGrassUI.Models;
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
        [HttpGet]
        public async Task<IActionResult> NotificationsList()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/Plant/GetUsersPlants{Request.Cookies["UserId"]}");
                if (response.IsSuccessStatusCode)
                {
                    var notificationsList = await response.Content.ReadFromJsonAsync<IEnumerable<PlantView>>();
                    var sortedNotifications = notificationsList
                        .OrderBy(notification => notification.NextWateringDate < notification.NextFertilizingDate ? 0 : 1)
                        .ThenBy(notification => notification.NextWateringDate)
                        .ToList();

                    return View(sortedNotifications);
                }
                else
                {
                    ViewBag.ErrorMessage = "Błąd serwera.";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNotification(NotificationDto notificationDto)
        {
            try
            {
                if(notificationDto.Id == 0)
                {
                    HttpResponseMessage notification = await _httpClient.PostAsJsonAsync($"api/Notification/PostNotification", notificationDto);
                    if (notification.IsSuccessStatusCode)
                    {
                        return RedirectToAction("GetPlants", "Plant");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Błąd serwera.";
                        return View("Error");
                    }
                }
                else
                {
                    HttpResponseMessage notification = await _httpClient.PutAsJsonAsync($"api/Notification/PutNotification", notificationDto);
                    if (notification.IsSuccessStatusCode)
                    {
                        return RedirectToAction("NotificationsList", "Notification");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Błąd serwera.";
                        return View("Error");
                    }
                }                                         
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UpdateWatering(int plantId)
        {
            try
            {
                HttpResponseMessage notification = await _httpClient.GetAsync($"api/Plant/UpdateWateringStatus{plantId}");
                if (notification.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetPlants", "Plant");
                }
                else
                {
                    ViewBag.ErrorMessage = "Błąd serwera.";
                    return View("Error");
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UpdateFertilizing(int plantId)
        {
            try
            {
                HttpResponseMessage notification = await _httpClient.GetAsync($"api/Plant/UpdateFertilizingStatus{plantId}");
                if (notification.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetPlants", "Plant");
                }
                else
                {
                    ViewBag.ErrorMessage = "Błąd serwera.";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }

        }

        [Authorize]
        public async Task<IActionResult> RemoveNotification(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/Notification/DeleteNotification{id}");

                if (response.IsSuccessStatusCode)
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
