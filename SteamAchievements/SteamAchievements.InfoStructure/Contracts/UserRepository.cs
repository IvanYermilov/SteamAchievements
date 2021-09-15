using SteamAchievements.InfoStructure.Entities;
using SteamAchievements.InfoStructure.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SteamAchievements.InfoStructure.Contracts
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(bool trackChanges) =>
           await FindAll(trackChanges)
                .OrderBy(e => e.FirstName)
                .ToListAsync();
    }
}
