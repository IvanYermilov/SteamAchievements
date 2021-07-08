﻿using AutoMapper;
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
using Newtonsoft.Json;

namespace SteamAchievements.Controllers
{
    [Route("api/developers")]
    [ApiController]
    public class DeveloperController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;


        public DeveloperController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "DeveloperById")]
        public async Task<IActionResult> GetDeveloper(Guid id)
        {
            var developer = await _repository.Developer.GetDeveloperAsync(id, trackChanges: false);
            if (developer == null)
            {
                _logger.LogInfo($"Developer with id: {id} doesn't exist in the database.");

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

            _repository.Developer.CreateDeveloper(developerEntity);
            await _repository.SaveAsync();

            var developerToReturn = _mapper.Map<DeveloperDto>(developerEntity);

            return CreatedAtRoute("DeveloperById", new { id = developerToReturn.Id }, developerToReturn);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateDeveloperExistsAttribute))]
        public async Task<IActionResult> DeleteDeveloper(Guid id)
        {
            HttpClient client = new HttpClient();
            var developer = HttpContext.Items["developer"] as Developer;
            IEnumerable<Game> games = default;

            try
            {
                HttpResponseMessage response = await client.GetAsync($"https://localhost:5001/api/developers/{id}/games");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                games = JsonConvert.DeserializeObject<IEnumerable<Game>>(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            foreach (var game in games)
            {
                var developersForGame = _repository.Developer.GetDevelopersForGameAsync(game.Id, trackChanges: false).Result;
                if (developersForGame.Count() == 1 && developersForGame.Any(d => d.Id.Equals(developer.Id))) _repository.Game.DeleteGame(game);
            }
            _repository.Developer.DeleteDeveloper(developer);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
