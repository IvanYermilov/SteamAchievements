using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SteamAchievements.Infrastructure.Contracts;
using SteamAchievements.Application.DataTransferObjects.Achievements;
using SteamAchievements.Infrastructure.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using SteamAchievements.Infrastructure.ActionFilters;

namespace SteamAchievements.Application.Controllers
{
    [Route("api/games/{gameId}/achievements")]
    [ApiController]
    public class AchievementController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public AchievementController(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetAchievementForGame")]
        [ServiceFilter(typeof(ValidateAchievementForGameExistsAttribute))]
        public IActionResult GetAchievementForGame(Guid gameId, Guid id)
        {
            var achievementForGame = HttpContext.Items["achievement"] as Achievement;

            var achievement = _mapper.Map<AchievementDto>(achievementForGame);

            return Ok(achievement);
        }

        [HttpGet]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        public IActionResult GetAchievementsForGame(Guid gameId)
        {
            var game = HttpContext.Items["game"] as Game;
            
            var achievements = game.Achievements.ToList();

            var achievementsDto = _mapper.Map<IEnumerable<AchievementDto>>(achievements);

            return Ok(achievementsDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        public async Task<IActionResult> CreateAchievementForGame(Guid gameId, [FromBody] AchievementForManipulationDto achievement)
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


        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateAchievementForGameExistsAttribute))]
        public async Task<IActionResult> DeleteAchievementForGame(Guid gameId, Guid id)
        {
            var achievementForGame = HttpContext.Items["achievement"] as Achievement;
            _repository.Achievement.Delete(achievementForGame);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
