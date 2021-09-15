using AutoMapper;
using SteamAchievements.Application.DataTransferObjects.Games;
using SteamAchievements.Infrastructure.Entities.Models;

namespace SteamAchievements.Application.MappingProfiles
{
    public class GameMappingProfile : Profile
    {
        public GameMappingProfile()
        {
            CreateMap<Game, GameDto>();

            CreateMap<GameForManipulationDto, Game>();

            CreateMap<GameForManipulationDto, Game>().ReverseMap();
        }
    }
}
