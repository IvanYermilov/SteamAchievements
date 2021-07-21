using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace Contracts
{
    public interface IDeveloperRepository
    {
        Task<Developer> GetDeveloperAsync(Guid developerId, bool trackChanges);
        Task<IEnumerable<Developer>> GetAllDevelopersAsync(bool trackChanges);
        Task<IEnumerable<Developer>> GetDevelopersForGameAsync(Guid gameId, bool trackChanges);
        void CreateDeveloper(Developer developer);
        void DeleteDeveloper(Developer developer);
        bool IsDeveloperExist(Guid developerId);
    }
}
