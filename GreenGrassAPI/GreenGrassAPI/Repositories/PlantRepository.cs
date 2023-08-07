using AutoMapper;
using GreenGrassAPI.Context;
using GreenGrassAPI.Dtos;
using GreenGrassAPI.Interfaces;
using GreenGrassAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenGrassAPI.Repositories
{
    public class PlantRepository : GenericRepository<Plant>, IPlantRepository
    {
        private readonly GreenGrassDbContext _context;
        private readonly IMapper _mapper;
        public PlantRepository(GreenGrassDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PlantView>> GetAllPlants()
        {
            var plants = await _context.Plants.ToListAsync();
            var plantDtos = _mapper.Map<List<PlantView>>(plants);
            foreach (var plant in plantDtos)
            {
                var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.PlantId == plant.Id);
                if (notification != null)
                {
                    plant.LastWateringDate = notification.LastWateringDate;
                    plant.NextFertilizingDate = notification.NextFertilizingDate;
                    plant.NextWateringDate = notification.NextWateringDate;
                    plant.LastFertilizingDate = notification.LastFertilizingDate;
                }
            }
            return plantDtos;
        }

        public async Task<List<PlantView>> GetAllUsersPlants(int userId)
        {
            var plants = await _context.Plants.Where(p => p.UserId == userId).OrderBy(p => p.DateAdded).ToListAsync();

            var plantDtos = _mapper.Map<List<PlantView>>(plants);
            foreach (var plant in plantDtos)
            {
                var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.PlantId == plant.Id);
                if (notification != null)
                {
                    var notificationDto = _mapper.Map<NotificationDto>(notification);
                    plant.NotificationDto = notificationDto;
                    plant.LastWateringDate = notification.LastWateringDate;
                    plant.NextFertilizingDate = notification.NextFertilizingDate;
                    plant.NextWateringDate = notification.NextWateringDate;
                    plant.LastFertilizingDate = notification.LastFertilizingDate;
                }              
            }
            return plantDtos;
        }

        public async Task<PlantDto> GetPlantById(int id)
        {
            var plant = await _context.Plants.FirstOrDefaultAsync(x => x.Id == id);
            var plantDto = _mapper.Map<PlantDto>(plant);

            return plantDto;
        }

        public async Task<PlantView> CreatePlant(PlantDto plantDto)
        {
            var plant = _mapper.Map<Plant>(plantDto);


            _context.Plants.Add(plant);
            await _context.SaveChangesAsync();
            return _mapper.Map<PlantView>(await _context.Plants.FirstOrDefaultAsync(x => x.Id == plant.Id));
        }
    }
}