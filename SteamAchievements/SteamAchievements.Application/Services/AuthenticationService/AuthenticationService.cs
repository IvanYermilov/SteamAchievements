using System;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SteamAchievements.Application.DataTransferObjects.Users;
using SteamAchievements.Infrastructure.Entities.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace SteamAchievements.Application.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private User _user;

        public AuthenticationService (IMapper mapper, UserManager<User> userManager, IConfiguration configuration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
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

        public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuth)
        {
            _user = await _userManager.FindByNameAsync(userForAuth.UserName);

            return (_user != null && await _userManager.CheckPasswordAsync(_user, userForAuth.Password));
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"));
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials
            signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var tokenOptions = new JwtSecurityToken
            (
                issuer: jwtSettings.GetSection("validIssuer").Value,
                audience: jwtSettings.GetSection("validAudience").Value,
                claims: claims,
                expires:
                DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("expires").Value)),
                signingCredentials: signingCredentials
            );

            return tokenOptions;
        }

    }
}