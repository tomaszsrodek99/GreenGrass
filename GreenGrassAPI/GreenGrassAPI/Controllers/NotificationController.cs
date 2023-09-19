using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GreenGrassAPI.Dtos;
using GreenGrassAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenGrassAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using GreenGrassAPI.Models;
using AutoMapper;

namespace GreenGrassAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : Controller
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IPlantRepository _plantRepository;
        private readonly IMapper _mapper;

        public NotificationController(INotificationRepository notificationRepository, IMapper mapper, IPlantRepository plantRepository)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
            _plantRepository = plantRepository;
        }

        [HttpGet]
        [Route("GetNotifications")]
        public async Task<ActionResult<List<NotificationDto>>> GetAllNotifications()
        {
            try
            {
                var notifications = await _notificationRepository.GetAllAsync();
                if (notifications.Any())
                {
                    return Ok(notifications);
                }
                return BadRequest("Brak powiadomień.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllNotificationForUser{userId}")]
        public async Task<ActionResult<List<NotificationDto>>> GetAllNotificationForUser(int userId)
        {
            try
            {
                var notifications = await _notificationRepository.GetAllNotificationsWithPlantsForUser(userId);
                if (notifications.Any())
                {
                    return Ok(notifications);
                }
                return BadRequest("Brak powiadomień dla podanego użytkownika");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetNotificationWithPlant{notificationId}")]
        public async Task<ActionResult<NotificationDto>> GetNotificationWithPlant(int notificationId)
        {
            try
            {
                var notification = await _notificationRepository.GetNotification(notificationId);
                if (notification == null)
                {
                    return NotFound("Brak powiadomienia o takim ID.");
                }
                return _mapper.Map<NotificationDto>(notification);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetNotificationForPlant{plantId}")]
        public async Task<ActionResult<NotificationDto>> GetNotificationForPlant(int plantId)
        {
            try
            {
                var notification = await _notificationRepository.GetNotificationForPlantByPlantId(plantId);
                if (notification == null)
                {
                    return NotFound("Brak powiadomienia dla podanej rośliny.");
                }
                return _mapper.Map<NotificationDto>(notification);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetNotificationsToDisplay{userId}")]
        public async Task<ActionResult<List<NotificationDto>>> GetNotificationsToDisplay(int userId)
        {
            try
            {
                var notification = await _notificationRepository.GetRequiredAllNotifications(userId);
                if (!notification.Any())
                {
                    return NotFound("Brak wymaganych powiadomień do wyświetlenia.");
                }
                return notification;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("PostNotification")]
        public async Task<ActionResult<Notification>> PostNotification(NotificationDto notificationDto)
        {
            try
            {
                var notification = _mapper.Map <Notification>(notificationDto);
                notification.LastFertilizingDate = DateTime.Now;
                notification.LastWateringDate = DateTime.Now;
                notification.FertilizingPeriod = (int)(notification.NextFertilizingDate.Date - notification.LastFertilizingDate.Date).TotalDays;
                notification.WateringPeriod = (int)(notification.NextWateringDate.Date - notification.LastWateringDate.Date).TotalDays;

                await _notificationRepository.AddAsync(notification);

                var plant = await _plantRepository.GetAsync(notificationDto.PlantId);

                plant.NotificationId = notification.Id;

                await _plantRepository.UpdateAsync(plant);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("PutNotification")]
        public async Task<IActionResult> PutNotification(NotificationDto notificationDto)
        {
            try
            {
                var notification = await _notificationRepository.GetAsync(notificationDto.Id);
                if (notification == null)
                {
                    return NotFound("Brak powiadomienia o podanym ID.");
                }
                if (notification.PlantId != notificationDto.PlantId)
                {
                    return BadRequest("Zmiana pola PlantId jest niedozwolona.");
                }

                notification.NextFertilizingDate = notificationDto.NextFertilizingDate;
                notification.NextWateringDate = notificationDto.NextWateringDate;

                notification.WateringPeriod = (int)(notificationDto.NextWateringDate - notification.LastWateringDate.Date).TotalDays;
                notification.FertilizingPeriod = (int)(notificationDto.NextFertilizingDate - notification.LastFertilizingDate.Date).TotalDays;

                await _notificationRepository.UpdateAsync(notification);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

        [HttpDelete("DeleteNotification{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            try
            {
                var notification = await _notificationRepository.GetAsync(id);
                var plant = await _plantRepository.GetAsync(notification.PlantId);

                plant.NotificationId = null;
                await _plantRepository.UpdateAsync(plant);

                if (notification == null)
                {
                    return NotFound("Brak powiadomienia o podanym ID.");
                }

                await _notificationRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}