using System;
using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class DeveloperRepository : RepositoryBase<Developer>, IDeveloperRepository
    {
        public DeveloperRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateDeveloper(Developer developer) => Create(developer);

        public async Task<Developer> GetDeveloperAsync(Guid developerId, bool trackChanges) =>
            await FindByCondition(d => d.Id.Equals(developerId), trackChanges)
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<Developer>> GetAllDevelopersAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                .OrderBy(d => d.Name)
                .ToListAsync();
    }
}
