using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SteamAchievements.Application.DataTransferObjects.Achievements;
using SteamAchievements.Infrastructure.Entities.Models;

namespace SteamAchievements.Application.Services.AchievementsService
{
    public interface IAchievementService
    {
        Achievement CurrentAchievement { get; set; }
        AchievementDto GetAchievement();
        public IEnumerable<AchievementDto> GetAchievementsForGame();
        public Task<AchievementDto> CreateAchievementForGame(Guid gameId,
            AchievementForManipulationDto achievement);
        public void DeleteAchievementForGame();
    }
}
