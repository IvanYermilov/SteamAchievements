using SteamAchievements.Application.DataTransferObjects.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteamAchievements.Application.Services.UserService
{
    public interface IUserService
    {
        public Task<IEnumerable<UserDto>> GetUsers();
    }
}