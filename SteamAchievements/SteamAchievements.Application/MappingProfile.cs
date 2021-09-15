using AutoMapper;
using DataTransferObjects.Developers;
using DataTransferObjects.Games;
using DataTransferObjects.Achievements;
using DataTransferObjects.Users;
using SteamAchievements.InfoStructure.Entities.Models;

namespace SteamAchievements
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

            CreateMap<GameForManipulationDto, Game>().ReverseMap();

            CreateMap<UserForManipulationDto, User>();

            CreateMap<User, UserDto>();
        }

    }
}
