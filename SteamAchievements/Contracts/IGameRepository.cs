using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace Contracts
{
    public interface IGameRepository
    {
        Task<IEnumerable<Game>> GetGamesAsync(Guid developerId, bool trackChanges);
        void CreateGameForDeveloper(Developer developer, Game game);
        Task<Game> GetGameAsync(Guid developerId, Guid id, bool trackChanges);
        void DeleteGame(Game game);
    }
}
