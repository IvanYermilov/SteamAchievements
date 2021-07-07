using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using SteamAchievements.ActionFilters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteamAchievements.Controllers
{
    [Route("api/developers/{developerId}/games")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public GameController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetGameForDeveloper")]
        [ServiceFilter(typeof(ValidateDeveloperForGameExistsAttribute))]
        public async Task<IActionResult> GetGameForCompany(Guid developerId, Guid id)
        {
            var gameDb = await _repository.Game.GetGameAsync(developerId, id, trackChanges: false);

            if (gameDb == null)
            {
                _logger.LogInfo($"Game with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            var employee = _mapper.Map<GameDto>(gameDb);

            return Ok(employee);
        }

        [HttpGet]
        public async Task<IActionResult> GetGamesForDeveloper(Guid developerId)
        {
            var developer = await _repository.Developer.GetDeveloperAsync(developerId, trackChanges: false);
            if (developer == null)
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
        public async Task<IActionResult> CreateGameForDeveloper(Guid developerId, [FromBody] GameForCreationDto game)
        {
            var gameEntity = _mapper.Map<Game>(game);
            var developerForGame = HttpContext.Items["developer"] as Developer;
            developerForGame.Games.Add(gameEntity);
            await _repository.SaveAsync();
            var gameToReturn = _mapper.Map<GameDto>(gameEntity);
            return CreatedAtRoute("GetGameForDeveloper", new
            {
                developerId,
                id = gameToReturn.Id
            }, gameToReturn);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateGameForDeveloperExistsAttribute))]
        public async Task<IActionResult> DeleteGameForDeveloper(Guid developerId, Guid id)
        {
            var gameForDeveloper = HttpContext.Items["game"] as Game;
            _repository.Game.DeleteGame(gameForDeveloper);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
