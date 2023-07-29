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
            var plants = _context.Plants.ToListAsync();
            var plantDtos = _mapper.Map<List<PlantView>>(plants);
            foreach (var plant in plantDtos)
            {
                var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.PlantId == plant.Id);
                plant.LastWateringDate = notification.LastWateringDate;
                plant.NextFertilizingDate = notification.NextFertilizingDate;
                plant.NextWateringDate = notification.NextWateringDate;
                plant.LastFertilizingDate = notification.LastFertilizingDate;
                plant.DaysUntilWatering = (int)(plant.NextWateringDate.Date - DateTime.Now.Date).TotalDays;
                plant.DaysUntilFertilizing = (int)(plant.NextFertilizingDate.Date - DateTime.Now.Date).TotalDays;
            }
            return plantDtos;
        }

        public async Task<List<PlantView>> GetAllUsersPlants(int userId)
        {
            var plants = _context.Plants.Where(p => p.UserId == userId).OrderBy(p => p.DateAdded).ToListAsync();
            var plantDtos = _mapper.Map<List<PlantView>>(plants);
            foreach (var plant in plantDtos)
            {
                var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.PlantId == plant.Id);
                plant.LastWateringDate = notification.LastWateringDate;
                plant.NextFertilizingDate = notification.NextFertilizingDate;
                plant.NextWateringDate = notification.NextWateringDate;
                plant.LastFertilizingDate = notification.LastFertilizingDate;
                plant.DaysUntilWatering = (int)(plant.NextWateringDate.Date - DateTime.Now.Date).TotalDays;
                plant.DaysUntilFertilizing = (int)(plant.NextFertilizingDate.Date - DateTime.Now.Date).TotalDays;
            }
            return plantDtos;
        }

        public async Task<PlantView> GetPlantById(int id)
        {
            var plant = _context.Plants.FirstOrDefaultAsync(x => x.Id == id);
            var plantDto = _mapper.Map<PlantView>(plant);
            var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.PlantId == id);

            plantDto.LastWateringDate = notification.LastWateringDate;
            plantDto.NextFertilizingDate = notification.NextFertilizingDate;
            plantDto.NextWateringDate = notification.NextWateringDate;
            plantDto.LastFertilizingDate = notification.LastFertilizingDate;
            plantDto.DaysUntilWatering = (int)(plantDto.NextWateringDate.Date - DateTime.Now.Date).TotalDays;
            plantDto.DaysUntilFertilizing = (int)(plantDto.NextFertilizingDate.Date - DateTime.Now.Date).TotalDays;
            return plantDto;
        }

        public async Task<PlantView> CreatePlant(PlantDto plantDto)
        {
            var plant = _mapper.Map<Plant>(plantDto);

            var notification = new Notification
            {
                PlantId = plant.Id,
                LastWateringDate = DateTime.MinValue,
                NextWateringDate = DateTime.MinValue,
                LastFertilizingDate = DateTime.MinValue,
                NextFertilizingDate = DateTime.MinValue
            };
            plant.Notification = notification;
            _context.Plants.Add(plant);
            await _context.SaveChangesAsync();
            return _mapper.Map<PlantView>(await _context.Plants.FirstOrDefaultAsync(x => x.Id == plant.Id));
        }
    }
}