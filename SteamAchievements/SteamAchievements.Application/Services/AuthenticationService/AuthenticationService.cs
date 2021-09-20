using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SteamAchievements.Application.DataTransferObjects.Users;
using SteamAchievements.Infrastructure.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteamAchievements.Application.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public AuthenticationService (IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<List<IdentityResult>> RegisterUser(UserForManipulationDto userForRegistration)
        {
            var result = new List<IdentityResult>();
            var user = _mapper.Map<User>(userForRegistration);

            var userCreationResult = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (userCreationResult.Succeeded)
            {
                var rolesToUserAdditionResult = await _userManager.AddToRolesAsync(user, userForRegistration.Roles);
                if (!rolesToUserAdditionResult.Succeeded)
                {
                    result.Add(rolesToUserAdditionResult);
                    var userDeletionResult = await _userManager.DeleteAsync(user);
                    if (!userDeletionResult.Succeeded) result.Add(userDeletionResult);
                }
            }
            else result.Add(userCreationResult);

            return result;
        }
    }
}