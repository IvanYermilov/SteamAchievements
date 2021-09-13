using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using SteamAchievements.ActionFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using SteamAchievements.Services;

namespace SteamAchievements.Controllers
{
    [Route("api/developers")]
    [ApiController]
    public class DeveloperController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly CurrentSessionStateService _currentSessionService;

        public DeveloperController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, CurrentSessionStateService currentSessionService)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _currentSessionService = currentSessionService;
        }

        [HttpGet("{developerId}", Name = "DeveloperById")]
        public async Task<IActionResult> GetDeveloper(Guid developerId)
        {
            var developer = await _repository.Developer.GetDeveloperAsync(developerId, trackChanges: false);
            if (developer == null)
            {
                _logger.LogInfo($"Developer with id: {developerId} doesn't exist in the database.");

                return NotFound();
            }
            else
            {
                var developerDto = _mapper.Map<DeveloperDto>(developer);
                return Ok(developerDto);
            }
        }

        [HttpGet("for-game-by/{id}")]
        public async Task<IActionResult> GetDevelopersForGame(Guid id)
        {
            var developer = await _repository.Developer.GetDevelopersForGameAsync(id, trackChanges: false);
            if (developer == null)
            {
                _logger.LogInfo($"Developers for game with id: {id} doesn't exist in the database.");

                return NotFound();
            }
            else
            {
                var developerDto = _mapper.Map<IEnumerable<DeveloperDto>>(developer);
                return Ok(developerDto);
            }
        }

        [HttpGet(Name = "GetDevelopers")]
        public async Task<IActionResult> GetAllDevelopers()
        {
            var developers = await _repository.Developer.GetAllDevelopersAsync(trackChanges: false);
            var developerDto = _mapper.Map<IEnumerable<DeveloperDto>>(developers);
            return Ok(developerDto);
        }

        [HttpPost(Name = "CreateDeveloper")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateDeveloper([FromBody] DeveloperForCreationDto developer)
        {
            var developerEntity = _mapper.Map<Developer>(developer);

            _repository.Developer.Create(developerEntity);
            await _repository.SaveAsync();

            var developerToReturn = _mapper.Map<DeveloperDto>(developerEntity);

            return CreatedAtRoute("DeveloperById", new { id = developerToReturn.Id }, developerToReturn);
        }

        [HttpDelete("{developerId}")]
        [ServiceFilter(typeof(ValidateDeveloperExistsAttribute))]
        public async Task<IActionResult> DeleteDeveloper(Guid developerId)
        {
            var developer = _currentSessionService.CurrentDeveloper;
            foreach (var game in developer.Games)
            {
                if (game.Developers.Count() == 1 && game.Developers.Any(d => d.Id.Equals(developer.Id))) _repository.Game.Delete(game);
            }
            _repository.Developer.Delete(developer);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
