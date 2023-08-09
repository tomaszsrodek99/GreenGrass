﻿using AutoMapper;
using GreenGrassAPI.Dtos;
using GreenGrassAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Numerics;

namespace GreenGrassUI.Controllers
{
    public class PlantController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PlantController(IWebHostEnvironment webHostEnvironment)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44304/");
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetPlants()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/Plant/GetUsersPlants{Request.Cookies["UserId"]}");
                if (response.IsSuccessStatusCode)
                {
                    var plants = await response.Content.ReadFromJsonAsync<IEnumerable<PlantView>>();
                    return View("PlantsList", plants);
                }
                else
                {
                    return View("PlantsList", new List<PlantView>());
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public IActionResult CreateView()
        {
            return View("CreatePlant");
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddPlant(PlantDto plantDto)
        {
            try
            {
                if (plantDto.ImageFile != null)
                    plantDto = AddImage(plantDto);

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Plant/CreatePlant", plantDto);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetPlants");
                }
                return View("CreatePlant", plantDto);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }
        private PlantDto AddImage(PlantDto plantDto)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string imagePath = Path.Combine(webRootPath, "PlantImages");

            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            string fileName = Path.GetFileName(plantDto.ImageFile.FileName);

            string filePath = Path.Combine(imagePath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                plantDto.ImageFile.CopyTo(fileStream);
            }

            plantDto.ImageUrl = Path.Combine("/PlantImages", fileName);
            return plantDto;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UpdateView(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/Plant/GetPlantById{id}");
            var plantDto = await response.Content.ReadFromJsonAsync<PlantDto>();
            return View("CreatePlant", plantDto);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditPlant(PlantDto plantDto)
        {
            try
            {
                if (plantDto.ImageFile != null)
                    plantDto = AddImage(plantDto);
                else
                {
                    HttpResponseMessage plant = await _httpClient.GetAsync($"api/Plant/GetPlantById{plantDto.Id}");
                    var currentPlant = await plant.Content.ReadFromJsonAsync<PlantDto>();
                    plantDto.ImageUrl = currentPlant.ImageUrl;
                }

                HttpResponseMessage response = await _httpClient.PutAsJsonAsync("api/Plant/UpdatePlant", plantDto);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetPlants");
                }
                return View("CreatePlant", plantDto);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public async Task<IActionResult> RemovePlant(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/Plant/DeletePlant{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetPlants");
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
        [HttpGet]
        public async Task<IActionResult> DetailsOfPlant(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/Plant/GetPlantById{id}");
            var plantDto = await response.Content.ReadFromJsonAsync<PlantDto>();
            ViewData["PlantName"] = plantDto.Name;
            return View("PlantDetails", plantDto);
        }
    }
}
