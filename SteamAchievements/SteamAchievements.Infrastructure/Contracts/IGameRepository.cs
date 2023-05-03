using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamAchievements.Infrastructure.Entities.Models;

namespace SteamAchievements.Infrastructure.Contracts
{
    public interface IGameRepository : IRepositoryBase<Game>
    {
        Task<IEnumerable<Game>> GetGamesAsync(Guid developerId, bool trackChanges);
        Task<IEnumerable<Game>> GetGamesForDeveloper(Guid devId);
        Task<IEnumerable<Game>> GetGamesForUserAsync(string userId, bool trackChanges);
        Task<Game> GetGameByIdAsync(Guid gameId, bool trackChanges);
        Task<Game> GetGameAsync(Guid developerId, Guid id, bool trackChanges);
    }
}
