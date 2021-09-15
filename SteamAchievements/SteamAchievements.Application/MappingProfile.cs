using AutoMapper;
using SteamAchievements.Application.DataTransferObjects.Achievements;
using SteamAchievements.Application.DataTransferObjects.Developers;
using SteamAchievements.Application.DataTransferObjects.Games;
using SteamAchievements.Application.DataTransferObjects.Users;
using SteamAchievements.Infrastructure.Entities.Models;

namespace SteamAchievements.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Developer, DeveloperDto>()
                .ForMember(d => d.FullAddress,
                    opt => opt
                        .MapFrom(x => string.Join(' ', x.Address, x.Country)));

            CreateMap<DeveloperForManipulationDto, Developer>();

            CreateMap<Game, GameDto>();

            CreateMap<GameForManipulationDto, Game>();

            CreateMap<Achievement, AchievementDto>();

            CreateMap<Achievement, AchievementDto>().ReverseMap();

            CreateMap<AchievementForManipulationDto, Achievement>();

            CreateMap<DeveloperForManipulationDto, Developer>().ReverseMap();
            
            CreateMap<Developer, DeveloperDto>();

            CreateMap<GameForManipulationDto, Game>().ReverseMap();

            CreateMap<UserForManipulationDto, User>();

            CreateMap<User, UserDto>();
        }

    }
}
