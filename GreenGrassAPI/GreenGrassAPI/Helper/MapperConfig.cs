using AutoMapper;
using GreenGrassAPI.Dtos;
using System.Reflection;

namespace GreenGrassAPI.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Models.Notification, NotificationDto>().ReverseMap();
            CreateMap<Models.Plant, PlantDto>().ReverseMap();
            CreateMap<Models.Plant, PlantView>().ReverseMap();
            CreateMap<Models.User, UserDto>().ReverseMap();
        }
    }
}