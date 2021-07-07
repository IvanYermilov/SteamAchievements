using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using SteamAchievements.ActionFilters;

namespace SteamAchievements.Controllers
{
    [Route("api/games/{gameId}/achievements")]
    [ApiController]
    public class AchievementController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public AchievementController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetAchievementForGame")]
        [ServiceFilter(typeof(ValidateGamerForAchievementExistsAttribute))]
        public async Task<IActionResult> GetAchievementForGame(Guid gameId, Guid id)
        {
            var achievementDb = await _repository.Game.GetGameAsync(gameId, id, trackChanges: false);

            if (achievementDb == null)
            {
                _logger.LogInfo($"Achievement with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            var achievement = _mapper.Map<AchievementDto>(achievementDb);

            return Ok(achievement);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetGamesForDeveloper(Guid developerId)
        //{
        //    var developer = await _repository.Developer.GetDeveloperAsync(developerId, trackChanges: false);
        //    if (developer == null)
        //    {
        //        _logger.LogInfo($"Developer with id: {developerId} doesn't exist in the database.");
        //        return NotFound();
        //    }
        //    var gamesFromDb = await _repository.Game.GetGamesAsync(developerId, trackChanges: false);

        //    var gamesDto = _mapper.Map<IEnumerable<GameDto>>(gamesFromDb);

        //    return Ok(gamesDto);
        //}

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateGamerForAchievementExistsAttribute))]
        public async Task<IActionResult> CreateAchievementForGame(Guid gameId, [FromBody] AchievementForCreationDto achievement)
        {
            var achievementEntity = _mapper.Map<Achievement>(achievement);
            _repository.Achievement.CreateAchievementForGame(gameId, achievementEntity);
            await _repository.SaveAsync();
            var achievementToReturn = _mapper.Map<AchievementDto>(achievementEntity);
            return CreatedAtRoute("GetAchievementForGame", new
            {
                gameId,
                id = achievementToReturn.Id
            }, achievementToReturn);
        }


        //[HttpDelete("{id}")]
        //[ServiceFilter(typeof(ValidateGameForDeveloperExistsAttribute))]
        //public async Task<IActionResult> DeleteGameForDeveloper(Guid developerId, Guid id)
        //{
        //    var gameForDeveloper = HttpContext.Items["game"] as Game;
        //    _repository.Game.DeleteGame(gameForDeveloper);
        //    await _repository.SaveAsync();
        //    return NoContent();
        //}
    }
}
