using AutoMapper;
using SteamAchievements.Application.DataTransferObjects.Achievements;
using SteamAchievements.Infrastructure.Entities.Models;

namespace SteamAchievements.Application.MappingProfiles
{
    public class AchievementMappingProfile : Profile
    {
        public AchievementMappingProfile()
        {

            CreateMap<Achievement, AchievementDto>();

            CreateMap<Achievement, AchievementDto>().ReverseMap();

            CreateMap<AchievementForManipulationDto, Achievement>();
        }
    }
}
