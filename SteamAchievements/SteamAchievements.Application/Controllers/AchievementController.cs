using Microsoft.AspNetCore.Mvc;
using SteamAchievements.Application.ActionFilters;
using SteamAchievements.Application.DataTransferObjects.Achievements;
using SteamAchievements.Application.Services.AchievementsService;
using System;
using System.Threading.Tasks;

namespace SteamAchievements.Application.Controllers
{
    [Route("api/games/{gameId}/achievements")]
    [ApiController]
    public class AchievementController : ControllerBase
    {
        private readonly IAchievementService _achievementService;

        public AchievementController(IAchievementService achievementService)
        {
            _achievementService = achievementService;
        }

        [HttpGet("{id}", Name = "GetAchievementForGame")]
        [ServiceFilter(typeof(ValidateAchievementForGameExistsAttribute))]
        public IActionResult GetAchievementForGame(Guid gameId, Guid id)
        {
            var achievement = _achievementService.GetAchievementsForGame();

            return Ok(achievement);
        }

        [HttpGet]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        public IActionResult GetAchievementsForGame(Guid gameId)
        {
            var achievementsDto = _achievementService.GetAchievementsForGame();

            return Ok(achievementsDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        public async Task<IActionResult> CreateAchievementForGame(Guid gameId, [FromBody] AchievementForManipulationDto achievement)
        {
            var achievementToReturn = await _achievementService.CreateAchievementForGame(gameId, achievement);
            return CreatedAtRoute("GetAchievementForGame", new
            {
                gameId,
                id = achievementToReturn.Id
            }, achievementToReturn);
        }


        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateAchievementForGameExistsAttribute))]
        public IActionResult DeleteAchievementForGame(Guid gameId, Guid id)
        {
            _achievementService.DeleteAchievementForGame();
            return NoContent();
        }
    }
}
