using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace Contracts
{
    public interface IGameRepository : IRepositoryBase<Game>
    {
        Task<IEnumerable<Game>> GetGamesAsync(Guid developerId, bool trackChanges);
        Task<IEnumerable<Game>> GetGamesForDeveloper(Guid devId);
        Task<Game> GetGameById(Guid gameId, bool trackChanges);
        Task<Game> GetGameAsync(Guid developerId, Guid id, bool trackChanges);
    }
}
