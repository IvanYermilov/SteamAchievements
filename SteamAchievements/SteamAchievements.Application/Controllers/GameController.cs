using AutoMapper;
using SteamAchievements.InfoStructure.Contracts;
using DataTransferObjects.Games;
using SteamAchievements.InfoStructure.Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SteamAchievements.InfoStructure.ActionFilters;
using SteamAchievements.InfoStructure.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteamAchievements.Application.Controllers
{
    [Route("api/developers/{developerId}/games")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly CurrentSessionStateService _currentSessionService;

        public GameController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, CurrentSessionStateService currentSessionService)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _currentSessionService = currentSessionService;
        }

        [HttpGet("{gameId}", Name = "GetGameForDeveloper")]
        public async Task<IActionResult> GetGameForDeveloper(Guid developerId, Guid gameId)
        {
            var isDeveloperExist = _repository.Developer.IsDeveloperExist(developerId);
            if (isDeveloperExist == false)
            {
                _logger.LogInfo($"Developer with id: {developerId} doesn't exist in the database.");
                return NotFound();
            }

            var gameDb = await _repository.Game.GetGameAsync(developerId, gameId, trackChanges: false);

            if (gameDb == null)
            {
                _logger.LogInfo($"Game with id: {gameId} doesn't exist in the database.");
                return NotFound();
            }

            var game = _mapper.Map<GameDto>(gameDb);

            return Ok(game);
        }

        [HttpGet]
        public async Task<IActionResult> GetGamesForDeveloper(Guid developerId)
        {
            var isDeveloperExist = _repository.Developer.IsDeveloperExist(developerId);
            if (isDeveloperExist == false)
            {
                _logger.LogInfo($"Developer with id: {developerId} doesn't exist in the database.");
                return NotFound();
            }
            var gamesFromDb = await _repository.Game.GetGamesAsync(developerId, trackChanges: false);

            var gamesDto = _mapper.Map<IEnumerable<GameDto>>(gamesFromDb);

            return Ok(gamesDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateDeveloperForGameExistsAttribute))]
        public async Task<IActionResult> CreateGameForDeveloper(Guid developerId, [FromBody] GameForManipulationDto game)
        {
            var gameEntity = _mapper.Map<Game>(game);
            var developerForGame = _currentSessionService.CurrentDeveloper;
            developerForGame.Games.Add(gameEntity);
            await _repository.SaveAsync();
            var gameToReturn = _mapper.Map<GameDto>(gameEntity);
            return CreatedAtRoute("GetGameForDeveloper", new
            {
                developerId,
                id = gameToReturn.Id
            }, gameToReturn);
        }

        [HttpPost("add-to-developer-game-with/{gameId}")]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        public async Task<IActionResult> AddGameForDeveloper(Guid developerId, Guid gameId)
        {

            var gameEntity = HttpContext.Items["game"] as Game;
            Developer developer = await _repository.Developer.GetDeveloperAsync(developerId, true);
            gameEntity.Developers.Add(developer);
            await _repository.SaveAsync();
            return Ok();
        }

        [HttpPatch("{gameId}")]
        [ServiceFilter(typeof(ValidateGameForDeveloperExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid developerId, Guid gameId, [FromBody] JsonPatchDocument<GameForManipulationDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var gameEntity = HttpContext.Items["game"] as Game;

            var gameToPatch = _mapper.Map<GameForManipulationDto>(gameEntity);

            patchDoc.ApplyTo(gameToPatch, ModelState);

            TryValidateModel(gameToPatch);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(gameToPatch, gameEntity);

            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpDelete("delete-from-developer-game-with/{gameId}")]
        [ServiceFilter(typeof(ValidateGameForDeveloperExistsAttribute))]
        public async Task<IActionResult> DetachGameForDeveloper(Guid developerId, Guid gameId)
        {
            var game = HttpContext.Items["game"] as Game;
            var developer = await _repository.Developer.GetDeveloperAsync(developerId, true);
            game.Developers.Remove(developer);
            await _repository.SaveAsync();
            return Ok();
        }

        [HttpDelete("{gameId}")]
        [ServiceFilter(typeof(ValidateGameForDeveloperExistsAttribute))]
        public async Task<IActionResult> DeleteGameForDeveloper(Guid developerId, Guid gameId)
        {
            var game = HttpContext.Items["game"] as Game;
            _repository.Game.Delete(game);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
