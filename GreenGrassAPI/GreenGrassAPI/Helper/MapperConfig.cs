using AutoMapper;
using System.Reflection;

namespace GreenGrassAPI.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Models.Notification, Dtos.NotificationDto>().ReverseMap();
            CreateMap<Models.Plant, Dtos.PlantDto>().ReverseMap();
            CreateMap<Models.Plant, Dtos.PlantView>().ReverseMap();
            CreateMap<Dtos.PlantDto, Dtos.PlantView>().ReverseMap();
            CreateMap<Models.User, Dtos.UserDto>().ReverseMap();
        }
    }
}