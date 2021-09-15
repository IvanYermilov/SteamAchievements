using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamAchievements.InfoStructure.Entities.Models;

namespace SteamAchievements.InfoStructure.Contracts
{
    public interface IDeveloperRepository : IRepositoryBase<Developer>
    {
        Task<Developer> GetDeveloperAsync(Guid developerId, bool trackChanges);
        Task<IEnumerable<Developer>> GetAllDevelopersAsync(bool trackChanges);
        Task<IEnumerable<Developer>> GetDevelopersForGameAsync(Guid gameId, bool trackChanges);
        bool IsDeveloperExist(Guid developerId);
    }
}
