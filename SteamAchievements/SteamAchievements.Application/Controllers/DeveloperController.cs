using Microsoft.AspNetCore.Mvc;
using SteamAchievements.Application.ActionFilters;
using SteamAchievements.Application.DataTransferObjects.Developers;
using SteamAchievements.Application.Services.DeveloperService;
using SteamAchievements.Infrastructure.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SteamAchievements.Application.Controllers
{
    [Route("api/developers")]
    [ApiController]
    public class DeveloperController : ControllerBase
    {
        private readonly IDeveloperService _developerService;

        public DeveloperController(IDeveloperService developerService)
        {
            _developerService = developerService;
        }

        [HttpGet("{developerId}", Name = "DeveloperById")]
        [ServiceFilter(typeof(ValidateDeveloperExistsAttribute))]
        public IActionResult GetDeveloper(Guid developerId)
        {
            var developer = _developerService.CurrentDeveloperDto;
            if (developer == null) return NotFound();
            else return Ok(developer);
        } 

        [HttpGet("for-game-by/{id}")]
        public async Task<IActionResult> GetDevelopersForGame(Guid id)
        {
            var developers = await _developerService.GetDevelopersForGame(id, trackChanges: false);
            if (developers == null) return NotFound();
            else return Ok(developers);
        }

        [HttpGet(Name = "GetDevelopers")]
        public async Task<IActionResult> GetAllDevelopers()
        {
            var developers = await _developerService.GetAllDevelopers(trackChanges: false);
            return Ok(developers);
        }

        [HttpPost(Name = "CreateDeveloper")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateDeveloper([FromBody] DeveloperForManipulationDto developer)
        {
            var createdDeveloper = await _developerService.CreateDeveloper(developer);

            return CreatedAtRoute("DeveloperById", new { developerId = createdDeveloper.Id }, createdDeveloper);
        }

        [HttpDelete("{developerId}")]
        [ServiceFilter(typeof(ValidateDeveloperExistsAttribute))]
        public IActionResult DeleteDeveloper(Guid developerId)
        {
            _developerService.DeleteDeveloper();
            return NoContent();
        }
    }
}
