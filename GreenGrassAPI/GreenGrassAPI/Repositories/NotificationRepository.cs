using AutoMapper;
using GreenGrassAPI.Context;
using GreenGrassAPI.Dtos;
using GreenGrassAPI.Interfaces;
using GreenGrassAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenGrassAPI.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly GreenGrassDbContext _context;
        private readonly IMapper _mapper;
        public NotificationRepository(GreenGrassDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task UpdateWateringStatusAsync(int plantId, DateTime currentDate)
        {
            var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.PlantId == plantId);
            var plant = await _context.Plants.FirstOrDefaultAsync(n => n.Id == plantId);
            if (notification != null)
            {
                notification.LastWateringDate = currentDate;
                notification.NextWateringDate = currentDate.AddDays((double)plant.WateringFrequency);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateFertilizingStatusAsync(int plantId, DateTime currentDate)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.PlantId == plantId);
            var plant = await _context.Plants.FirstOrDefaultAsync(n => n.Id == plantId);
            if (notification != null)
            {
                notification.LastFertilizingDate = currentDate;
                notification.NextFertilizingDate = currentDate.AddDays((double)plant.FertilizingFrequency);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<NotificationDto>> GetAllNotificationsWithPlants()
        {
            var notifications = await _context.Notifications.ToListAsync();
            var notificationsDto = _mapper.Map<List<NotificationDto>>(notifications);

            foreach (var notificationDto in notificationsDto)
            {
                var plant = await _context.Plants.FirstOrDefaultAsync(n => n.Id == notificationDto.PlantId);
                var plantView = _mapper.Map<PlantView>(plant);
                plantView.LastWateringDate = notificationDto.LastWateringDate;
                plantView.NextFertilizingDate = notificationDto.NextFertilizingDate;
                plantView.NextWateringDate = notificationDto.NextWateringDate;
                plantView.LastFertilizingDate = notificationDto.LastFertilizingDate;
                notificationDto.Plant = plantView;
            }
            return notificationsDto;
        }

        public async Task<List<NotificationDto>> GetAllNotificationsWithPlantsForUser(int id)
        {
            var notificationsDto = await GetAllNotificationsWithPlants();
            return notificationsDto.Where(x => x.Plant.UserId == id).ToList();
        }
        public async Task<NotificationDto> GetNotification(int id)
        {
            var notification = _mapper.Map<NotificationDto>(await _context.Notifications.FindAsync(id));
            if (notification == null)
            {
                return null;
            }
            var plant = await _context.Plants.FirstOrDefaultAsync(n => n.NotificationId == notification.Id);
            var plantView = _mapper.Map<PlantView>(plant);
            plantView.LastWateringDate = notification.LastWateringDate;
            plantView.NextFertilizingDate = notification.NextFertilizingDate;
            plantView.NextWateringDate = notification.NextWateringDate;
            plantView.LastFertilizingDate = notification.LastFertilizingDate;
            notification.Plant = plantView;
            return notification;
        }
        public async Task<NotificationDto> GetNotificationForPlantByPlantId(int id)
        {
            var plant = await _context.Plants.FindAsync(id);
            if (plant == null)
            {
                return null;
            }
            return await GetNotification((int)plant.NotificationId);
        }

        public async Task<List<NotificationDto>> GetRequiredAllNotifications(int userId)
        {
            var notificationDtos = await GetAllNotificationsWithPlantsForUser(userId);
            notificationDtos = notificationDtos.Where(n => n.Plant.DaysUntilWatering <= 0 || n.Plant.DaysUntilFertilizing <= 0).ToList();
            return notificationDtos;
        }
    }
}