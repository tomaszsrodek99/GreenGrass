﻿using GreenGrassAPI.Dtos;
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
    }
}