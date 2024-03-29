﻿using SteamAchievements.Infrastructure.Entities.Models;
using SteamAchievements.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SteamAchievements.Infrastructure.Contracts
{
    public class GameRepository : RepositoryBase<Game>, IGameRepository
    {
        public GameRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Game>> GetGamesAsync(Guid developerId, bool trackChanges) =>
            await FindByCondition(g => g.Developers.Any(d => d.Id.Equals(developerId)), trackChanges)
                .OrderBy(g => g.Name)
                .ToListAsync();

        public async Task<IEnumerable<Game>> GetGamesForDeveloper(Guid devId)
        {
            return await RepositoryContext.Games.Where(g => g.Developers.Any(d => d.Id == devId)).Include(g => g.Developers).ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetGamesForUserAsync(string userId, bool trackChanges)
        {
            return await FindByCondition(g => g.Users.Any(u => u.Id == userId), trackChanges)
                .OrderBy(g => g.Name)
                .ToListAsync();
        }

        public async Task<Game> GetGameByIdAsync(Guid gameId, bool trackChanges)
        {
            return await FindByCondition(g => g.Id == gameId, trackChanges)
                .Include(g => g.Developers)
                .Include(g => g.Achievements)
                .SingleOrDefaultAsync();
        }

        public async Task<Game> GetGameAsync(Guid developerId, Guid id, bool trackChanges) =>
            await FindByCondition(g => g.Developers.FirstOrDefault(d => d.Id == developerId) != null 
                                       && g.Id.Equals(id), trackChanges)
                .Include(g => g.Developers)
                .Include(g => g.Achievements)
                .SingleOrDefaultAsync();
    }
}
