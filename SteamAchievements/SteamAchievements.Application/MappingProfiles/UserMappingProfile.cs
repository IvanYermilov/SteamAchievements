using AutoMapper;
using SteamAchievements.Application.DataTransferObjects.Users;
using SteamAchievements.Infrastructure.Entities.Models;

namespace SteamAchievements.Application.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserForManipulationDto, User>();

            CreateMap<User, UserDto>();
        }
    }
}
