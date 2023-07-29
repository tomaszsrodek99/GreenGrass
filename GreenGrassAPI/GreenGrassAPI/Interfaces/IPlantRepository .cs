using GreenGrassAPI.Dtos;
using GreenGrassAPI.Models;

namespace GreenGrassAPI.Interfaces
{
    public interface IPlantRepository : IGenericRepository<Plant>
    {
        Task<List<PlantView>> GetAllPlants();
        Task<List<PlantView>> GetAllUsersPlants(int userId);
        Task<PlantView> CreatePlant(PlantDto plantDto);
        Task<PlantView> GetPlantById(int id);
    }
}