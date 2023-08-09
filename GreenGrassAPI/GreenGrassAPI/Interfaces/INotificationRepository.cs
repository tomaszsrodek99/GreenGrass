using GreenGrassAPI.Dtos;
using GreenGrassAPI.Models;

namespace GreenGrassAPI.Interfaces
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task UpdateWateringStatusAsync(int plantId);
        Task UpdateFertilizingStatusAsync(int plantId);
        Task<List<NotificationDto>> GetAllNotificationsWithPlants();
        Task<List<NotificationDto>> GetAllNotificationsWithPlantsForUser(int id);
        Task<NotificationDto> GetNotification(int id);
        Task<NotificationDto> GetNotificationForPlantByPlantId(int id);
        Task<List<NotificationDto>> GetRequiredAllNotifications(int userId);
    }
}