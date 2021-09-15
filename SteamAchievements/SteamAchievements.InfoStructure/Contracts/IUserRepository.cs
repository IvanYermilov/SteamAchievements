using SteamAchievements.InfoStructure.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteamAchievements.InfoStructure.Contracts
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<IEnumerable<User>> GetAllUsersAsync(bool trackChanges);
    }
}
