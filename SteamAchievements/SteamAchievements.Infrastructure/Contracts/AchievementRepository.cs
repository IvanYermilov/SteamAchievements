using SteamAchievements.Infrastructure.Entities.Models;
using SteamAchievements.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SteamAchievements.Infrastructure.Contracts
{
    public class AchievementRepository : RepositoryBase<Achievement>, IAchievementRepository
    {
        public AchievementRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        public async Task<IEnumerable<Achievement>> GetAchievementsAsync(Guid gameId, bool trackChanges) =>
            await FindByCondition(a => a.GameId.Equals(gameId), trackChanges)
                .OrderBy(a => a.Name)
                .ToListAsync();

        public async Task<Achievement> GetAchievementAsync(Guid gameId, Guid id, bool trackChanges) =>
            await FindByCondition(a => a.GameId.Equals(gameId) && a.Id.Equals(id), trackChanges)
                .SingleOrDefaultAsync();

        public void CreateAchievementForGame(Guid gameId, Achievement achievement)
        {
            achievement.GameId = gameId;
            Create(achievement);
        }
    }
}
