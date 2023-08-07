using AutoMapper;
using GreenGrassAPI.Dtos;
using GreenGrassAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
            plantDto.UserId = int.Parse(Request.Cookies["UserId"]);
            try
            {
                plantDto = AddImage(plantDto);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Plant/CreatePlant", plantDto);

                if (response.IsSuccessStatusCode)
                {
                    var createdPlant = await response.Content.ReadFromJsonAsync<Plant>();
                    return RedirectToAction("GetPlants");
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("Name", errorMessage);
                    return View("CreatePlant", plantDto);
                }
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
    }
}
