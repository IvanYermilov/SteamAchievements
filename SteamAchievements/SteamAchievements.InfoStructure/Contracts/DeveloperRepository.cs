using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts
{
    public class DeveloperRepository : RepositoryBase<Developer>, IDeveloperRepository
    {
        public DeveloperRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<Developer> GetDeveloperAsync(Guid developerId, bool trackChanges) =>
            await FindByCondition(d => d.Id.Equals(developerId), trackChanges)
                .Include(d => d.Games)
                .SingleOrDefaultAsync();
        public async Task<IEnumerable<Developer>> GetDevelopersForGameAsync(Guid gameId, bool trackChanges) =>
            await FindByCondition(d => d.Games.Any(g => g.Id == gameId), trackChanges)
                .OrderBy(d => d.Name)
                .Include(d => d.Games)
                .ToListAsync();

        public async Task<IEnumerable<Developer>> GetAllDevelopersAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                .OrderBy(d => d.Name)
                .ToListAsync();

        public bool IsDeveloperExist(Guid developerId)
        {
            var result = RepositoryContext.Developer.Any(d => d.Id == developerId);
            return result;
        }
    }
}
