using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SteamAchievements.ActionFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;

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
        public async Task<IActionResult> GetGameForDeveloper(Guid developerId, Guid id)
        {
            var gameDb = await _repository.Game.GetGameAsync(developerId, id, trackChanges: false);

            if (gameDb == null)
            {
                _logger.LogInfo($"Game with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            var game = _mapper.Map<GameDto>(gameDb);

            return Ok(game);
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

            IEnumerable<Game> games = await _repository.Game.GetGamesForDeveloper(developerId);

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

        [HttpPost("add-to-developer-game-with/{id}")]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        public async Task<IActionResult> AddGameForDeveloper(Guid developerId, Guid id)
        {

            var gameEntity = HttpContext.Items["game"] as Game;
            Developer developer = await _repository.Developer.GetDeveloperAsync(developerId, true);
            gameEntity.Developers.Add(developer);
            await _repository.SaveAsync();
            return Ok();
        }

        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidateGameForDeveloperExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid developerId, Guid id, [FromBody] JsonPatchDocument<GameForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var gameEntity = HttpContext.Items["game"] as Game;

            var gameToPatch = _mapper.Map<GameForUpdateDto>(gameEntity);

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

        [HttpDelete("delete-from-developer-game-with/{id}")]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        public async Task<IActionResult> DetachGameForDeveloper(Guid developerId, Guid id)
        {

            var gameEntity = HttpContext.Items["game"] as Game;
            Game game;
            Developer developer = default;
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync($"https://localhost:5001/api/developers/{developerId}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var developerDto = JsonConvert.DeserializeObject<DeveloperDto>(responseBody);
                if (developerDto == null)
                {
                    _logger.LogInfo($"Developer with id: {id} doesn't exist.");
                    return NotFound();
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            game = await _repository.Game.GetGameAsync(developerId, id, true);
            developer = await _repository.Developer.GetDeveloperAsync(developerId, true);
            game.Developers.Remove(developer);
            await _repository.SaveAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateGameForDeveloperExistsAttribute))]
        public async Task<IActionResult> DeleteGameForDeveloper(Guid developerId, Guid id)
        {
            HttpClient client = new HttpClient();
            var gameForDeveloper = HttpContext.Items["game"] as Game;
            IEnumerable<Achievement> achievements = default;
            try
            {
                HttpResponseMessage response = await client.GetAsync($"https://localhost:5001/api/games/{id}/achievements");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var achievementsDto = JsonConvert.DeserializeObject<IEnumerable<AchievementDto>>(responseBody);
                achievements = _mapper.Map<IEnumerable<Achievement>>(achievementsDto);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            foreach (var achievement in achievements)
            {   

                 _repository.Achievement.DeleteAchievement(achievement);
            }
            
            _repository.Game.DeleteGame(gameForDeveloper);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
