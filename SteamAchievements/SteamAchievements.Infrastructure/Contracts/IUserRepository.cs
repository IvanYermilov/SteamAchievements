using SteamAchievements.Infrastructure.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteamAchievements.Infrastructure.Contracts
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<IEnumerable<User>> GetAllUsersAsync(bool trackChanges);
    }
}
