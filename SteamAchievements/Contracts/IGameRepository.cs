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
        //Task<IEnumerable<Game>> GetAllGamesAsync(bool trackChanges);
        void CreateGameForDeveloper(Developer developer, Game game);
    }
}
