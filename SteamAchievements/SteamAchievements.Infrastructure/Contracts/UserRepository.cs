using SteamAchievements.Infrastructure.Entities;
using SteamAchievements.Infrastructure.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SteamAchievements.Infrastructure.Contracts
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
