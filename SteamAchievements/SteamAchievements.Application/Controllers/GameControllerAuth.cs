using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SteamAchievements.Application.ActionFilters;
using SteamAchievements.Application.Services.GameService;

namespace SteamAchievements.Application.Controllers
{
    [Route("api/authGames")]
    [ApiController]
    public class GameControllerAuth : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameControllerAuth(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet, Authorize]
        [ServiceFilter(typeof(ValidateDeveloperExistsAuthAttribute))]
        public async Task<IActionResult> GetGameForDeveloper(string userId)
        {
            var games = await _gameService.GetGamesForUserAsync(userId);
            return Ok(games);
        }
    }
}
