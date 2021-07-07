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

            CreateMap<AchievementForCreationDto, Achievement>();

            //CreateMap<EmployeeForUpdateDto, Employee>();

            //CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();

            //CreateMap<CompanyForUpdateDto, Company>();

            CreateMap<UserForRegistrationDto, User>();
            CreateMap<User, UserDto>();
        }

    }
}
