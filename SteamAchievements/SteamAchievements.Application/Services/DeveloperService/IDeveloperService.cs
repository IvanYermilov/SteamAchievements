using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SteamAchievements.Application.DataTransferObjects.Developers;
using SteamAchievements.Infrastructure.Entities.Models;

namespace SteamAchievements.Application.Services.DeveloperService
{
    public interface IDeveloperService
    {
        public Developer CurrentDeveloper { get; set; }
        public DeveloperDto CurrentDeveloperDto { get; }
        public Task<Developer> GetDeveloperByIdAsync(Guid developerId, bool trackChanges);
        public Task<IEnumerable<DeveloperDto>> GetDevelopersForGame(Guid id, bool trackChanges);
        public Task<IEnumerable<DeveloperDto>> GetAllDevelopers(bool trackChanges);
        public Task<DeveloperDto> CreateDeveloper(DeveloperForManipulationDto developer);
        public void DeleteDeveloper();
    }
}