using SteamAchievements.InfoStructure.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteamAchievements.InfoStructure.Contracts
{
    public interface IAchievementRepository : IRepositoryBase<Achievement>
    {
        Task<IEnumerable<Achievement>> GetAchievementsAsync(Guid gameId, bool trackChanges);
        void CreateAchievementForGame(Guid gameId, Achievement achievement);
        Task<Achievement> GetAchievementAsync(Guid gameId, Guid id, bool trackChanges);
    }
}
