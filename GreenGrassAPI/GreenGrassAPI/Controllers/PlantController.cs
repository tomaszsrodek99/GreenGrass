using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GreenGrassAPI.Dtos;
using GreenGrassAPI.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.UserSecrets;
using GreenGrassAPI.Interfaces;
using GreenGrassAPI.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace GreenGrassAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantController : Controller
    {
        private readonly IPlantRepository _plantRepository;
        private readonly IMapper _mapper;
        private readonly INotificationRepository _notificationRepository;

        public PlantController(IPlantRepository plantRepository, INotificationRepository notificationRepository, IMapper mapper)
        {
            _plantRepository = plantRepository;
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetAllPlants")]
        public async Task<ActionResult<List<PlantView>>> GetAllPlants()
        {
            try
            {
                var plants = await _plantRepository.GetAllPlants();
                if (!plants.Any())
                {
                    return BadRequest("Brak roślin w bazie danych.");
                }
                return plants;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUsersPlants{userId}")]
        public async Task<ActionResult<List<PlantView>>> GetUsersPlants(int userId)
        {
            try
            {
                var plants = await _plantRepository.GetAllUsersPlants(userId);
                if (!plants.Any())
                {
                    return BadRequest("Brak roślin w bazie danych.");
                }
                return plants;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetPlantById{plantId}")]
        public async Task<ActionResult<PlantDto>> GetPlantById(int plantId)
        {
            try
            {
                return await _plantRepository.GetPlantById(plantId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("CreatePlant")]
        public async Task<ActionResult<Plant>> CreatePlant(PlantDto plantDto)
        {
            try
            {             
                var created = await _plantRepository.CreatePlant(plantDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdatePlant")]
        public async Task<IActionResult> UpdatePlant(PlantDto plantDto)
        {
            try
            {
                var plant = _mapper.Map<Plant>(plantDto);
                await _plantRepository.UpdateAsync(plant);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeletePlant{id}")]
        public async Task<IActionResult> DeletePlant(int id)
        {
            try
            {
                await _plantRepository.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("UpdateWateringStatus{plantId}")]
        public async Task<IActionResult> UpdateWateringStatus(int plantId)
        {
            try
            {
                await _notificationRepository.UpdateWateringStatusAsync(plantId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("UpdateFertilizingStatus{plantId}")]
        public async Task<IActionResult> UpdateFertilizingStatus(int plantId)
        {
            try
            {
                await _notificationRepository.UpdateFertilizingStatusAsync(plantId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("CheckNotifications{userId}")]
        public async Task<IActionResult> CheckNotifications(int userId)
        {
            var plants = await _notificationRepository.GetAllNotificationsWithPlantsForUser(userId);
            List<NotificationDto> notifications = new();

            foreach (var plant in plants)
            {
                if (plant.Plant.LastWateringDate != null && plant.Plant.DaysUntilWatering <= 0)
                {
                    notifications.Add(plant);
                }

                if (plant.Plant.LastFertilizingDate != null && plant.Plant.DaysUntilFertilizing <= 0)
                {
                    notifications.Add(plant);
                }
            }
            return Ok(notifications);
        }
    }
}
