using AutoMapper;
using SteamAchievements.Application.DataTransferObjects.Developers;
using SteamAchievements.Infrastructure.Entities.Models;

namespace SteamAchievements.Application.MappingProfiles
{
    public class DeveloperMappingProfile : Profile
    {
        public DeveloperMappingProfile()
        {
            CreateMap<Developer, DeveloperDto>()
                .ForMember(d => d.FullAddress,
                    opt => opt
                        .MapFrom(x => string.Join(' ', x.Address, x.Country)));

            CreateMap<DeveloperForManipulationDto, Developer>();

            CreateMap<DeveloperForManipulationDto, Developer>().ReverseMap();
        }
    }
}
