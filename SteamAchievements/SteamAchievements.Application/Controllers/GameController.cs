using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SteamAchievements.Application.ActionFilters;
using SteamAchievements.Application.DataTransferObjects.Games;
using SteamAchievements.Application.Services.GameService;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SteamAchievements.Infrastructure.Contracts;

namespace SteamAchievements.Application.Controllers
{
    [Route("api/developers/{developerId}/games")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly ILoggerManager _logger;

        public GameController(IGameService gameService, ILoggerManager logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        [HttpGet("{gameId}", Name = "GetGameForDeveloper"), Authorize(Roles = "Manager")]
        [ServiceFilter(typeof(ValidateGameForDeveloperExistsAttribute))]
        public IActionResult GetGameForDeveloper(Guid developerId, Guid gameId)
        {
            var game = _gameService.CurrentGameDto;

            return Ok(game);
        }

        [HttpGet, Authorize]
        [ServiceFilter(typeof(ValidateDeveloperExistsAttribute))]
        public async Task<IActionResult> GetGamesForDeveloper(Guid developerId)
        {
            var games = await _gameService.GetGamesForDeveloper();
            return Ok(games);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateDeveloperForGameExistsAttribute))]
        public async Task<IActionResult> CreateGameForDeveloper(Guid developerId, [FromBody] GameForManipulationDto game)
        {
            var gameToReturn = await _gameService.CreateGameForDeveloper(game);
            return CreatedAtRoute("GetGameForDeveloper", new
            {
                developerId,
                gameId = gameToReturn.Id
            }, gameToReturn);
        }

        [HttpPost("add-to-developer-game-with/{gameId}")]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        [ServiceFilter(typeof(ValidateDeveloperExistsAttribute))]
        public IActionResult AddGameForDeveloper(Guid developerId, Guid gameId)
        {
            _gameService.AddGameForDeveloper();
            return Ok();
        }

        [HttpPatch("{gameId}")]
        [ServiceFilter(typeof(ValidateGameForDeveloperExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid developerId, Guid gameId, [FromBody] JsonPatchDocument<GameForPatchDto> patchDoc)
        {
            var isPatchDocNull = _gameService.CheckPatchDocIsNull(patchDoc);
            if (isPatchDocNull) return BadRequest("patchDoc object is null");
            var gameToPatch = _gameService.GetGameForPatch();
            patchDoc.ApplyTo(gameToPatch, ModelState);
            TryValidateModel(gameToPatch);
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }
            await _gameService.PartiallyUpdateGameAsync(gameToPatch);
            return NoContent();
        }

        [HttpDelete("delete-from-developer-game-with/{gameId}")]
        [ServiceFilter(typeof(ValidateGameForDeveloperExistsAttribute))]
        public IActionResult DetachGameForDeveloper(Guid developerId, Guid gameId)
        {
            Task.Run(() => _gameService.DetachGameForDeveloper(developerId));
            return Ok();
        }

        [HttpDelete("{gameId}")]
        [ServiceFilter(typeof(ValidateGameForDeveloperExistsAttribute))]
        public IActionResult DeleteGameForDeveloper(Guid developerId, Guid gameId)
        {
            Task.Run(() => _gameService.DeleteGameForDeveloper());
            return NoContent();
        }
    }
}
