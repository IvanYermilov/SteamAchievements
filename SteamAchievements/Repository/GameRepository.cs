using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
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

        public async Task<IEnumerable<Game>> GetGamesforDeveloper(Guid devId)
        {
            //return await RepositoryContext.Developer.FirstOrDefault(d => d.Id.Equals(devId)).Select(d => d.Games).ToListAsync();
            return await RepositoryContext.Games.Where(g => g.Developers.Any(d => d.Id == devId)).Include(g => g.Developers).ToListAsync();
        }

        public async Task<Game> GetGameAsync(Guid developerId, Guid id, bool trackChanges) =>
            await FindByCondition(g => g.Developers.FirstOrDefault(d => d.Id == developerId) != null 
                                       && g.Id.Equals(id), trackChanges)
                .SingleOrDefaultAsync();
        public void DeleteGame(Game game) => Delete(game);
    }
}
