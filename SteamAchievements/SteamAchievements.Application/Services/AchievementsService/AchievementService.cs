using AutoMapper;
using Microsoft.AspNetCore.Http;
using SteamAchievements.Application.DataTransferObjects.Achievements;
using SteamAchievements.Application.Services.RepositoryManager;
using SteamAchievements.Infrastructure.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SteamAchievements.Application.Services.AchievementsService
{
    public class AchievementService : IAchievementService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Achievement CurrentAchievement { get; set; }

        public AchievementService(IRepositoryManager repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public AchievementDto GetAchievement()
        {
            var achievement = _mapper.Map<AchievementDto>(CurrentAchievement);

            return achievement;
        }

        public IEnumerable<AchievementDto> GetAchievementsForGame()
        {
            var game = _httpContextAccessor.HttpContext.Items["game"] as Game;
            var achievements = game.Achievements.ToList();
            var achievementsDto = _mapper.Map<IEnumerable<AchievementDto>>(achievements);
            return achievementsDto;
        }

        public async Task<AchievementDto> CreateAchievementForGame(Guid gameId, AchievementForManipulationDto achievement)
        {
            var achievementEntity = _mapper.Map<Achievement>(achievement);
            _repository.Achievement.CreateAchievementForGame(gameId, achievementEntity);
            await _repository.SaveAsync();
            var achievementToReturn = _mapper.Map<AchievementDto>(achievementEntity);
            return achievementToReturn;
        }

        public async void DeleteAchievementForGame()
        {
            _repository.Achievement.Delete(CurrentAchievement);
            await _repository.SaveAsync();
        }
    }
}
