using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

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

            CreateMap<DeveloperForCreationDto, Developer>();

            CreateMap<Game, GameDto>();

            CreateMap<GameForCreationDto, Game>();

            CreateMap<Achievement, AchievementDto>();

            CreateMap<Achievement, AchievementDto>().ReverseMap();

            CreateMap<AchievementForCreationDto, Achievement>();

            CreateMap<DeveloperForUpdateDto, Developer>();

            CreateMap<DeveloperForUpdateDto, Developer>().ReverseMap();

            CreateMap<GameForUpdateDto, Game>();

            CreateMap<GameForUpdateDto, Game>().ReverseMap();

            CreateMap<UserForRegistrationDto, User>();
            CreateMap<User, UserDto>();
        }

    }
}
