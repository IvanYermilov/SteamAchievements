using AutoMapper;
using Microsoft.Extensions.Logging;
using SteamAchievements.Application.DataTransferObjects.Developers;
using SteamAchievements.Application.Services.RepositoryManager;
using SteamAchievements.Infrastructure.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SteamAchievements.Application.Services.DeveloperService
{
    public class DeveloperService : IDeveloperService
    {
        public Developer CurrentDeveloper { get; set; }
        public DeveloperDto CurrentDeveloperDto => _mapper.Map<DeveloperDto>(CurrentDeveloper);
        private readonly IRepositoryManager _repository;
        private readonly ILogger<DeveloperService> _logger;
        private readonly IMapper _mapper;

        public DeveloperService(IRepositoryManager repository, ILogger<DeveloperService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Developer> GetDeveloperByIdAsync(Guid developerId, bool trackChanges)
        {
            var developer = await _repository.Developer.GetDeveloperAsync(developerId, trackChanges);
            if (developer == null)
            {
                _logger.LogInformation($"Developer with id: {developerId} doesn't exist in the database.");
                return null;
            }
            else
            {
                var developerDto = _mapper.Map<DeveloperDto>(developer);
                return developer;
            }
        }
        public async Task<IEnumerable<DeveloperDto>> GetDevelopersForGame(Guid id, bool trackChanges)
        {
            var developers = await _repository.Developer.GetDevelopersForGameAsync(id, trackChanges: false);
            if (developers == null)
            {
                _logger.LogInformation($"Developers for game with id: {id} doesn't exist in the database.");
                return null;
            }
            else
            { 
                var developersDto = _mapper.Map<IEnumerable<DeveloperDto>>(developers);
                return developersDto;
            }
        }

        public async Task<IEnumerable<DeveloperDto>> GetAllDevelopers(bool trackChanges)
        {
            var developers = await _repository.Developer.GetAllDevelopersAsync(trackChanges: false);
            if (developers == null)
            {
                _logger.LogInformation($"There are no any developers at the database.");
                return null;
            }
            else
            {
                var developersDto = _mapper.Map<IEnumerable<DeveloperDto>>(developers);
                return developersDto;
            }
        }

        public async Task<DeveloperDto> CreateDeveloper(DeveloperForManipulationDto developer)
        {
            var developerEntity = _mapper.Map<Developer>(developer);
            _repository.Developer.Create(developerEntity);
            await _repository.SaveAsync();
            return _mapper.Map<DeveloperDto>(developerEntity);
        }

        public async void DeleteDeveloper()
        {
            foreach (var game in CurrentDeveloper.Games)
            {
                if (game.Developers.Count() == 1 && 
                    game.Developers.Any(d => d.Id.Equals(CurrentDeveloper.Id))) _repository.Game.Delete(game);
            }
            _repository.Developer.Delete(CurrentDeveloper);
            await _repository.SaveAsync();
        }
    }
}
