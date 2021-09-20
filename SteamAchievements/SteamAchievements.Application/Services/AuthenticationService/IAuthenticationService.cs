using Microsoft.AspNetCore.Identity;
using SteamAchievements.Application.DataTransferObjects.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteamAchievements.Application.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        public Task<List<IdentityResult>> RegisterUser(UserForManipulationDto userForRegistration);
    }
}